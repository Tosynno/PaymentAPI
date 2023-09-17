using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Models.Response
{
    public class ApiResponseBase<TData> : BaseResponseModel, IApiResponse
    {
        public virtual TData Data { get; set; }
        public ApiResponseBase()
        {
            try
            {
                if (typeof(TData).IsClass)
                    Data = (TData)Activator.CreateInstance(typeof(TData));
            }
            catch
            {
                //do nothing
            }
        }
    }

    public class ApiResponseBaseList<TData> : BaseResponseModel, IApiResponse
    {
        public virtual List<TData> Data { get; set; } = new List<TData>();
    }

    public class ApiResponseNoData : ApiResponseBase<object>
    {
        public string Response { get; set; }
    }
}
