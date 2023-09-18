using FluentValidation;
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
        private readonly IValidator<IntraBankTransferRequest> _validatorIntraBankTransferReq;
        private readonly IValidator<NIPTransactionRequest> _validatorNIPTransferReq;
        private readonly IRepository<PaymentdbContext, tbl_Account> _tblAccountStatementRepository;
        private readonly IRepository<PaymentdbContext, tbl_Marchant> _tblPaymentProfileRepository;
        private readonly IRepository<PaymentdbContext, tbl_NIPTransaction> _tblPaymentNIPRepository;

        public TransactionRepo(PaymentdbContext dbContext, IValidator<IntraBankTransferRequest> validatorIntraBankTransferReq, IRepository<PaymentdbContext, tbl_Account> tbl_AccountStatementRepository, IRepository<PaymentdbContext, tbl_Marchant> tblPaymentProfileRepository, IValidator<NIPTransactionRequest> validatorNIPTransferReq, IRepository<PaymentdbContext, tbl_NIPTransaction> tblPaymentNIPRepository) : base(dbContext)
        {
            _validatorIntraBankTransferReq = validatorIntraBankTransferReq;
            _tblAccountStatementRepository = tbl_AccountStatementRepository;
            _tblPaymentProfileRepository = tblPaymentProfileRepository;
            _validatorNIPTransferReq = validatorNIPTransferReq;
            _tblPaymentNIPRepository = tblPaymentNIPRepository;
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
            sadebit.ProfileId = resultProfiledebit.Id;
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
            sacredit.ProfileId = resultProfilecredit.Id;
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
            sacredit.ProfileId = resultProfil.Id;
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
    }
}
