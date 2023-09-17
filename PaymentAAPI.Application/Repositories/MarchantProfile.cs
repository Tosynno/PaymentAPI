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
    public class MarchantProfile : BaseRepository<PaymentdbContext, tbl_PaymentProfile>, IMarchantProfile
    {
        private readonly IValidator<PaymentProfileRequest> _validatorprofileReq;
        private readonly IValidator<UpdatePaymentProfileRequest> _validatorUpdateprofileReq;
        private readonly IValidator<AverageTransactionRequest> _validatorAverageTransactionReq;
        public MarchantProfile(PaymentdbContext dbContext, IValidator<PaymentProfileRequest> validatorprofileReq, IValidator<UpdatePaymentProfileRequest> validatorUpdateprofileReq, IValidator<AverageTransactionRequest> validatorAverageTransactionReq) : base(dbContext)
        {
            _validatorprofileReq = validatorprofileReq;
            _validatorUpdateprofileReq = validatorUpdateprofileReq;
            _validatorAverageTransactionReq = validatorAverageTransactionReq;
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
            tbl_PaymentProfile sa = new tbl_PaymentProfile();
            sa.BusinessId = request.BusinessId;
            sa.MerchantNumber = marc;
            sa.Surname = request.Surname;
            sa.ContactSurname = request.ContactSurname;
            sa.ContactName = request.ContactName;
            sa.DateOfEstablishment = request.DateOfEstablishment;
            sa.NationalIDNumber = request.NationalIDNumber;
            sa.Name = request.Name;
            sa.Surname = request.Surname;
            sa.IsActive = true;
            sa.CreatedDate =DateTime.Now;

            await AddAsync(sa);

            var metadat = new
            {
                MerchantNumber = marc,
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
            var chk = res.FirstOrDefault(c => c.MerchantNumber == request.MarchantNumber);
            if (chk == null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Marchant does not exist!!!";
                return response;
            }
            chk.BusinessId = request.BusinessId;
            chk.Surname = request.Surname;
            chk.ContactSurname = request.ContactSurname;
            chk.ContactName = request.ContactName;
            chk.DateOfEstablishment = request.DateOfEstablishment;
            chk.NationalIDNumber = request.NationalIDNumber;
            chk.Name = request.Name;
            chk.Surname = request.Surname;
            chk.IsActive = true;
            chk.CreatedDate = DateTime.Now;

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
                              MerchantNumber = u.MerchantNumber,
                              AverageTransaction = u.AverageTransaction,
                              NationalIDNumber = u.NationalIDNumber,
                              Name = u.Name,
                              Surname = u.Surname,
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
                    .ThenBy(c => c.Name).LastPage(pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => c.CreatedDate)
                    .ThenBy(c => c.Name).CountOfPages(pageSize),
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
                    .ThenBy(c => c.Name).FirstPage(pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => c.CreatedDate)
                    .ThenBy(c => c.Name).CountOfPages(pageSize),
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
                    .ThenBy(c => c.Name).Page(pageIndex, pageSize).ToList(),
                        Count = result.AsQueryable().OrderBy(c => c.CreatedDate)
                  .ThenBy(c => c.Name).CountOfPages(pageSize),
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
            var result = res.FirstOrDefault(c => c.MerchantNumber == request.MarchantNumber);
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
