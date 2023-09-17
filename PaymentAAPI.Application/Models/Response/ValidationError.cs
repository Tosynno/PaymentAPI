using PaymentAPI.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Models.Response
{
    public class ValidationError
    {
        public string Error { get; set; }
        public string Description { get; set; }

        public ValidationError()
        {
            Error = ErrorConstants.ERROR_MSG;
            Description = ErrorConstants.ERROR_MSG;
        }

        public ValidationError(string error, string description)
        {
            Error = error;
            Description = description;
        }

        public ValidationError(string error)
        {
            Error = error;
            Description = error;
        }
    }
}
