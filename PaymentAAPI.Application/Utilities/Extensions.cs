using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Utilities
{
    public static class Extensions
    {
        public static string ToUserFriendlyError(this string error)
        {
            if (string.IsNullOrEmpty(error))
                return "";
            var result = error;

            //Check if less than 60 characters
            if (result.Length <= 60)
                return result;

            //Check for allowed string
            string[] allowed = new string[] { "successful", "0700", "iFuelCONTACT" };
            foreach (var a in allowed)
            {
                if (result.ToLower().Contains(a?.ToLower()))
                {
                    return result;
                }
            }

            return ErrorConstants.ERROR_MSG;
        }
    }
}
