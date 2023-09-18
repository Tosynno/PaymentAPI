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
    public class CustomerRepo : BaseRepository<PaymentdbContext, tbl_Customer>, ICustomerRepo
    {
        private readonly IRepository<PaymentdbContext, tbl_Account> _tbl_Accountrepo;
        private readonly IValidator<CreateCustomerRequest> _validatorCreateCustomerReq;
        public CustomerRepo(PaymentdbContext dbContext, IRepository<PaymentdbContext, tbl_Account> tbl_Accountrepo, IValidator<CreateCustomerRequest> validatorCreateCustomerReq) : base(dbContext)
        {
            _tbl_Accountrepo = tbl_Accountrepo;
            _validatorCreateCustomerReq = validatorCreateCustomerReq;
        }

        public async Task<ApiResponseBase<object>> CreateCustomer(CreateCustomerRequest request)
        {
            var response = new ApiResponseBase<object>();
            var validationResult = await _validatorCreateCustomerReq.ValidateAsync(request);
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
            var chk = res.FirstOrDefault(c => c.CustomerNumber == request.CustomerNumber);
            if (chk != null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Customer already exist!!!";
                return response;
            }
            string marc = Utils.GenerateMarchantNumber();
            tbl_Customer sa = new tbl_Customer();
            sa.CustomerNumber = request.CustomerNumber;
            sa.NationalIDNumber = request.NationalIDNumber;
            sa.Surname = request.Surname;
            sa.Name = request.Name;
            sa.DateofBirth= request.DateofBirth;
            sa.CreatedDate= DateTime.Now;

            sa.IsActive = true;
            sa.CreatedDate = DateTime.Now;

            await AddAsync(sa);

            tbl_Account tbl_Account = new tbl_Account();
            tbl_Account.AccountNumber = marc;
            tbl_Account.AccountName = request.Surname + " " + request.Name;
            tbl_Account.ProfileId = request.CustomerNumber;
            tbl_Account.Date = DateTime.Now;
            await _tbl_Accountrepo.AddAsync(tbl_Account);
            var metadat = new
            {
                AccountName = tbl_Account.AccountName,
                AccountNumber = marc,
            };
            response.Data = metadat;
            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }
    }
}
