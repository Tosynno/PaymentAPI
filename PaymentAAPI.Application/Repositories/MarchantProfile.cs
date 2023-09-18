using FluentValidation;
using PaymentAPI.Application.Dto;
using PaymentAPI.Application.Helpers;
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
    public class MarchantProfile : BaseRepository<PaymentdbContext, tbl_Marchant>, IMarchantProfile
    {
        private readonly IRepository<PaymentdbContext, tbl_Account> _tbl_Accountrepo;
        private readonly IValidator<PaymentProfileRequest> _validatorprofileReq;
        private readonly IValidator<UpdatePaymentProfileRequest> _validatorUpdateprofileReq;
        private readonly IValidator<AverageTransactionRequest> _validatorAverageTransactionReq;
        public MarchantProfile(PaymentdbContext dbContext, IValidator<PaymentProfileRequest> validatorprofileReq, IValidator<UpdatePaymentProfileRequest> validatorUpdateprofileReq, IValidator<AverageTransactionRequest> validatorAverageTransactionReq, IRepository<PaymentdbContext, tbl_Account> tbl_Accountrepo) : base(dbContext)
        {
            _validatorprofileReq = validatorprofileReq;
            _validatorUpdateprofileReq = validatorUpdateprofileReq;
            _validatorAverageTransactionReq = validatorAverageTransactionReq;
            _tbl_Accountrepo = tbl_Accountrepo;
        }

        public async Task<ApiResponseBase<object>> CreateMarchant(PaymentProfileRequest request)
        {
            var response = new ApiResponseBase<object>();
            var validationResult = await _validatorprofileReq.ValidateAsync(request);
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
            var chk = res.FirstOrDefault(c => c.BusinessId == request.BusinessId);
            if (chk != null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Marchant already exist!!!";
                return response;
            }
            string marc = Utils.GenerateMarchantNumber();
            tbl_Marchant sa = new tbl_Marchant();
            sa.BusinessId = request.BusinessId;
            sa.BusinessName = request.BusinessName;
          
          
            sa.ContactSurname = request.ContactSurname;
            sa.ContactName = request.ContactName;
            sa.DateOfEstablishment = request.DateOfEstablishment;
           
            sa.IsActive = true;
            sa.CreatedDate =DateTime.Now;

            await AddAsync(sa);

            tbl_Account tbl_Account = new tbl_Account();
            tbl_Account.AccountNumber = marc;
            tbl_Account.AccountName = request.BusinessName;
            tbl_Account.ProfileId = request.BusinessId;
            tbl_Account.Date = DateTime.Now;
            await _tbl_Accountrepo.AddAsync(tbl_Account);
            var metadat = new
            {
                AccountName = request.BusinessName,
                AccountNumber = marc,
            };
            response.Data = metadat;
            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }

        public async Task<ApiResponseBase<object>> EditMarchant(UpdatePaymentProfileRequest request)
        {
            var response = new ApiResponseBase<object>();
            var validationResult = await _validatorUpdateprofileReq.ValidateAsync(request);
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
            var chk = res.FirstOrDefault(c => c.BusinessId == request.BusinessId);
            if (chk == null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Marchant does not exist!!!";
                return response;
            }
            chk.BusinessId = request.BusinessId;
           
            chk.ContactSurname = request.ContactSurname;
            chk.ContactName = request.ContactName;
            chk.DateOfEstablishment = request.DateOfEstablishment;
            
            chk.IsActive = true;
            chk.ModifyDate = DateTime.Now;

            await UpdateAsync(chk);

           
            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }

        public async Task<ApiResponseBase<object>> GetAllMarchant(int pageIndex, int pageSize, bool previous, bool next)
        {
            var response = new ApiResponseBase<object>();
            var res = await GetAllAsync();
            var result = (from u in res
                          select new PaymentProfileDto
                          {
                              BusinessId = u.BusinessId,
                              BusinessName = u.BusinessName,
                              ContactName = u.ContactName,
                              ContactSurname = u.ContactSurname,
                              DateOfEstablishment = u.DateOfEstablishment,
                              AverageTransaction = u.AverageTransaction,
                              Status = u.IsActive == true ? "ACTIVE" : "NOT ACTIVE",
                              CreatedDate = u.CreatedDate,
                              ModifyDate = u.ModifyDate,

                          }).ToList();
            if (result.Count() > 0)
            {
                if (previous)
                {
                    var metadata = new
                    {
                        Data = result.AsQueryable().OrderBy(c => c.CreatedDate)
                    .ThenBy(c => c.BusinessName).LastPage(pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => c.CreatedDate)
                    .ThenBy(c => c.BusinessName).CountOfPages(pageSize),
                    };
                    response.Data = metadata;
                    response.ResponseCode = "00";
                    response.ResponseDescription = "SUCCESSFUL";
                }
                else if (next)
                {
                    var metadata = new
                    {
                        Data = result.AsQueryable().OrderBy(c => c.CreatedDate)
                    .ThenBy(c => c.BusinessName).FirstPage(pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => c.CreatedDate)
                    .ThenBy(c => c.BusinessName).CountOfPages(pageSize),
                    };
                    response.Data = metadata;
                    response.ResponseCode = "00";
                    response.ResponseDescription = "SUCCESSFUL";
                }
                else
                {
                    var metadata = new
                    {
                        Data = result.AsQueryable().OrderBy(c => c.CreatedDate)
                    .ThenBy(c => c.BusinessName).Page(pageIndex, pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => c.CreatedDate)
                  .ThenBy(c => c.BusinessName).CountOfPages(pageSize),
                    };
                    response.Data = metadata;
                    response.ResponseCode = "00";
                    response.ResponseDescription = "SUCCESSFUL";
                }

            }
            else
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "No Records Found";
            }

            return response;
        }

        public async Task<ApiResponseBase<object>> SetAverageTransaction(AverageTransactionRequest request)
        {
            var response = new ApiResponseBase<object>();
            var validationResult = await _validatorAverageTransactionReq.ValidateAsync(request);
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
            var result = res.FirstOrDefault(c => c.BusinessId == request.BusinessId);
            if (result == null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Marchant does not exist!!!";
                return response;
            }
            result.AverageTransaction = request.TransactionAmountlimit;
            await UpdateAsync(result);
            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }
    }
}
