using FluentValidation;
using PaymentAPI.Application.Dto;
using PaymentAPI.Application.Interface;
using PaymentAPI.Application.Models;
using PaymentAPI.Application.Models.Response;
using PaymentAPI.Application.Services;
using PaymentAPI.Application.Utilities;
using PaymentAPI.Domain.Entities;
using PaymentAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Repositories
{
    public class TransactionRepo : BaseRepository<PaymentdbContext, tbl_PaymentTransaction>, ITransaction
    {
        private readonly IRepository<PaymentdbContext, tbl_Account> _tbl_Accountrepo;
        private readonly IRepository<PaymentdbContext, tbl_PaymentTransaction> _tbl_paymentTranrepo;
        private readonly IValidator<IntraBankTransferRequest> _validatorIntraBankTransferReq;
        private readonly IValidator<NIPTransactionRequest> _validatorNIPTransferReq;
        private readonly IRepository<PaymentdbContext, tbl_Account> _tblAccountStatementRepository;
        private readonly IRepository<PaymentdbContext, tbl_Marchant> _tblPaymentProfileRepository;
        private readonly IRepository<PaymentdbContext, tbl_NIPTransaction> _tblPaymentNIPRepository;

        public TransactionRepo(PaymentdbContext dbContext, IValidator<IntraBankTransferRequest> validatorIntraBankTransferReq, IRepository<PaymentdbContext, tbl_Account> tbl_AccountStatementRepository, IRepository<PaymentdbContext, tbl_Marchant> tblPaymentProfileRepository, IValidator<NIPTransactionRequest> validatorNIPTransferReq, IRepository<PaymentdbContext, tbl_NIPTransaction> tblPaymentNIPRepository, IRepository<PaymentdbContext, tbl_Account> tbl_Accountrepo, IRepository<PaymentdbContext, tbl_PaymentTransaction> tbl_paymentTranrepo) : base(dbContext)
        {
            _validatorIntraBankTransferReq = validatorIntraBankTransferReq;
            _tblAccountStatementRepository = tbl_AccountStatementRepository;
            _tblPaymentProfileRepository = tblPaymentProfileRepository;
            _validatorNIPTransferReq = validatorNIPTransferReq;
            _tblPaymentNIPRepository = tblPaymentNIPRepository;
            _tbl_Accountrepo = tbl_Accountrepo;
            _tbl_paymentTranrepo = tbl_paymentTranrepo;
        }

        public async Task<ApiResponseBase<object>> BalanceSheetLoader()
        {
            var response = new ApiResponseBase<object>();
            var res = await _tbl_Accountrepo.GetAllAsync();
            var resaccount = await _tbl_paymentTranrepo.GetAllAsync();
            var result = res.ToList();
            foreach ( var item in result ) { 
              

                var totaldebit = resaccount.Where(c => c.TransactionType =='D'
                                   && c.ProfileId == item.ProfileId).Select(c => c.TransactionAmount).Sum();
                var totalcredit = resaccount.Where(c => c.TransactionType == 'C'
                                   && c.ProfileId == item.ProfileId).Select(c => c.TransactionAmount).Sum();
                var availablebalance = resaccount.Where(c => 
                                    c.ProfileId == item.ProfileId).Select(c => c.Balance).Sum();
                var update = res.FirstOrDefault(c =>
                                    c.ProfileId == item.ProfileId);
                if ( update != null ) {
                    update.TotalDebit   = totaldebit;
                    update.TotalCredit = totalcredit;
                    update.AvailableBalance = availablebalance;
                    update.ClosingBalance = availablebalance;

                    await _tbl_Accountrepo.UpdateAsync(update);
                }
            }
            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }

        public async Task<ApiResponseBase<object>> IntraBankTransfer(IntraBankTransferRequest request)
        {
            var response = new ApiResponseBase<object>();
            var Profileres = await _tblPaymentProfileRepository.GetAllAsync();
            var Acctstatementres = await _tblAccountStatementRepository.GetAllAsync();
            var validationResult = await _validatorIntraBankTransferReq.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    response.ResponseCode = "99";
                    response.ResponseDescription = failure.ErrorMessage;
                    return response;
                }
            }
            var res = await GetAllAsync();

            var resultProfiledebit = Acctstatementres.FirstOrDefault(c => c.AccountNumber == request.DebitAccountNumber);
            if (resultProfiledebit == null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = $"{request.DebitAccountNumber} Does not exist!!!";
                return response;
            }
            var resultProfilecredit = Acctstatementres.FirstOrDefault(c => c.AccountNumber == request.CreditAccountNumber);
            if (resultProfilecredit == null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = $"{request.CreditAccountNumber} Does not exist!!!";
                return response;
            }

            var result = res.FirstOrDefault(c => c.Narration == request.Narration &&
          c.CreditAccountNumber == request.CreditAccountNumber && c.DebitAccountNumber == request.DebitAccountNumber);
            if (result != null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Duplicate Transaction!!!";
                return response;
            }

            var chkDebitMerchantNumber = res.FirstOrDefault(c => c.DebitAccountNumber == request.DebitAccountNumber);
            if (chkDebitMerchantNumber != null)
            {
                if (chkDebitMerchantNumber.Balance < request.TransactionAmount)
                {
                    response.ResponseCode = "99";
                    response.ResponseDescription = "Insufficient fund!!!";
                    return response;
                }
            }
          

            tbl_PaymentTransaction sadebit = new tbl_PaymentTransaction();
            sadebit.Tran_id = Utils.GenerateTranId();
            sadebit.Part_tran_srl_num = Utils.GeneratePart_tran_srl_num();
            sadebit.ProfileId = resultProfiledebit.ProfileId;
            sadebit.CreditAccountNumber = request.CreditAccountNumber;
            sadebit.TransactionType = 'D';
            sadebit.DebitAccountNumber = request.DebitAccountNumber;
            sadebit.TransactionAmount = request.TransactionAmount;
            sadebit.Balance = request.TransactionAmount - result.Balance;
            sadebit.Narration = request.Narration == null ? request.CreditAccountNumber + "_" + DateTime.Now.ToString("yyyyddMMhhmmss") : request.Narration;
            sadebit.TransactionDate = DateTime.Now;
            sadebit.ValueDate = DateTime.Now;
            await AddAsync(sadebit);
            

            tbl_PaymentTransaction sacredit = new tbl_PaymentTransaction();
            sacredit.Tran_id = sadebit.Tran_id;
            sacredit.Part_tran_srl_num = sadebit.Part_tran_srl_num;
            sacredit.ProfileId = resultProfilecredit.ProfileId;
            sacredit.CreditAccountNumber = request.CreditAccountNumber;
            sacredit.TransactionType = 'C';
            sacredit.DebitAccountNumber = request.DebitAccountNumber;
            sacredit.TransactionAmount = request.TransactionAmount;
            sacredit.Balance = request.TransactionAmount + result.Balance;
            sacredit.Narration = request.Narration == null ? request.CreditAccountNumber + "_" + DateTime.Now.ToString("yyyyddMMhhmmss") : request.Narration;
            sacredit.TransactionDate = DateTime.Now;
            sacredit.ValueDate = DateTime.Now;
            await AddAsync(sacredit);

            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }

        public async Task<ApiResponseBase<object>> NIPTransfer(NIPTransactionRequest request)
        {
            var response = new ApiResponseBase<object>();
            var Profileres = await _tblPaymentProfileRepository.GetAllAsync();
            var Acctstatementres = await _tblAccountStatementRepository.GetAllAsync();
            var validationResult = await _validatorNIPTransferReq.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    response.ResponseCode = "99";
                    response.ResponseDescription = failure.ErrorMessage;
                    return response;
                }
            }
            var res = await GetAllAsync();

            var resultProfil = Acctstatementres.FirstOrDefault(c => c.AccountNumber == request.DebitAccountNumber);
            if (resultProfil == null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = $"{request.DebitAccountNumber} Does not exist!!!";
                return response;
            }
           

            var result = res.FirstOrDefault(c => c.Narration == request.Narration &&
          c.CreditAccountNumber == request.CreditMerchantNumber && c.DebitAccountNumber == request.DebitAccountNumber);
            if (result != null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Duplicate Transaction!!!";
                return response;
            }


            tbl_PaymentTransaction sacredit = new tbl_PaymentTransaction();
            sacredit.Tran_id = Utils.GenerateTranId();
            sacredit.Part_tran_srl_num = Utils.GeneratePart_tran_srl_num();
            sacredit.ProfileId = resultProfil.ProfileId;
            sacredit.CreditAccountNumber = request.CreditMerchantNumber;
            sacredit.TransactionType = 'D';
            //sacredit.DebitAccountNumber = request.DebitAccountNumber;
            sacredit.TransactionAmount = request.TransactionAmount;
            sacredit.Balance = request.TransactionAmount - result.Balance;
            sacredit.Narration = request.Narration == null ? request.DebitAccountNumber + "_" + request.BankName : request.Narration;
            sacredit.TransactionDate = DateTime.Now;
            sacredit.ValueDate = DateTime.Now;
            await AddAsync(sacredit);

            tbl_NIPTransaction tbl_NIP = new tbl_NIPTransaction();
            tbl_NIP.PaymentTransactionId = sacredit.Id;
            tbl_NIP.CreditMerchantNumber = request.CreditMerchantNumber;
            tbl_NIP.TransactionType = 'D';
            tbl_NIP.DebitMerchantNumber = request.DebitAccountNumber;
            tbl_NIP.TransactionAmount = request.TransactionAmount;
            tbl_NIP.BankName = request.BankName;
            tbl_NIP.BankCode = request.BankCode;
            tbl_NIP.Narration = request.Narration == null ? request.DebitAccountNumber + "_" + request.BankName : request.Narration;
            tbl_NIP.TransactionDate = DateTime.Now;
            tbl_NIP.ValueDate = DateTime.Now;
            await _tblPaymentNIPRepository.AddAsync(tbl_NIP);

            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }

        public async Task<ApiResponseBase<object>> VerifyAccount(string AccountNumber)
        {
            var response = new ApiResponseBase<object>();
            var res = await _tbl_Accountrepo.GetAllAsync();
            var result = res.FirstOrDefault(c => c.AccountNumber == AccountNumber);
            if (result == null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Invalid Account Number!!!";
                return response;
            }
            var metadata = new
            {
                AccountName = result.AccountName,
                AccountNumber = result.AccountNumber,
                AvailableBalance = result.AvailableBalance,
                Status = result.IsActive == true ?"ACTIVE":"INACTIVE"
            };
            response.Data = metadata;
            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }
    }
}
