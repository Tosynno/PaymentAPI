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
    public interface ICustomerRepo : IRepository<PaymentdbContext, tbl_Customer>
    {
        Task<ApiResponseBase<object>> CreateCustomer(CreateCustomerRequest request);
        Task<ApiResponseBase<object>> GetAllCustomer(int pageIndex, int pageSize, bool previous, bool next);
    }
}
