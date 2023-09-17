using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Utilities
{
    public static class ErrorConstants
    {
        public const string ERROR_MSG = "An Error occurred. Please contact the Administrator.";
        public const string NOT_FOUND = "Record not found.";
        public const string DUPLICATE_RECORD = "Duplicate record.";
        public const string NULL_RESPONSE = "API returned null response.";
    }
}
