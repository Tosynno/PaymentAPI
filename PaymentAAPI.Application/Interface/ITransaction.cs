using PaymentAPI.Application.Models.Response;
using PaymentAPI.Application.Models;
using PaymentAPI.Domain.Entities;
using PaymentAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Interface
{
    public interface ITransaction : IRepository<PaymentdbContext, tbl_PaymentTransaction>
    {
        Task<ApiResponseBase<object>> IntraBankTransfer(IntraBankTransferRequest request); 
        Task<ApiResponseBase<object>> NIPTransfer(NIPTransactionRequest request); 
    }
}
