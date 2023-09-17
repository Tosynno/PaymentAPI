using PaymentAPI.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Models.Response
{
    public class BaseResponseModel
    {
        public bool Successful => !Errors.Any();
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();

        public void AddError(string Error)
        {
            Errors.Add(new ValidationError(Error));
        }

        public void AddErrors(string[] _Errors)
        {
            foreach (string Error in _Errors)
                Errors.Add(new ValidationError(Error));
        }

        public void AddError(string Error, string Description)
        {
            Errors.Add(new ValidationError(Error, Description));
        }

        public void MakeErrorFriendly()
        {
            if (Errors.Any())
            {
                foreach (var er in Errors)
                {
                    er.Error = er.Error.ToUserFriendlyError();
                    er.Description = er.Description.ToUserFriendlyError();
                }
            }
        }
    }
}
