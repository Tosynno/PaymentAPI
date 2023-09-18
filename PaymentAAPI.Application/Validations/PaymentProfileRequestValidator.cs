using FluentValidation;
using PaymentAPI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validations
{
    public class PaymentProfileRequestValidator : AbstractValidator<PaymentProfileRequest>
    {
        public PaymentProfileRequestValidator()
        {
            RuleFor(x => x.BusinessId).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.BusinessName)
    .NotEmpty().WithMessage("Enter a valid value");
        
            RuleFor(x => x.ContactName)
  .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ContactSurname)
  .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.DateOfEstablishment).Must(BeWithinOneYear)
            .WithMessage("the business age may not be less than 1 year");
         
        }

        private bool BeWithinOneYear(DateTime dateTime)
        {
            var r = dateTime <= DateTime.Now;
            return r;
        }
    }


}
