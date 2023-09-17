using PaymentAPI.Application.Dto;
using PaymentAPI.Application.Models;
using PaymentAPI.Application.Models.Response;
using PaymentAPI.Domain.Entities;
using PaymentAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Interface
{
    public interface IMarchantProfile : IRepository<PaymentdbContext, tbl_PaymentProfile>
    {
        Task<ApiResponseBase<object>> CreateMarchant(PaymentProfileRequest request);
        Task<ApiResponseBase<object>> EditMarchant(UpdatePaymentProfileRequest request);
        Task<ApiResponseBase<object>> GetAllMarchant(int pageIndex, int pageSize, bool previous, bool next);
        Task<ApiResponseBase<object>> SetAverageTransaction(AverageTransactionRequest request);
    }
}
