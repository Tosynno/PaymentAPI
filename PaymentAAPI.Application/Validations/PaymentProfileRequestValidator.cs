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
            RuleFor(x => x.BusinessId).LessThanOrEqualTo(0).WithMessage("Enter a valid value");
            RuleFor(x => x.BusinessName)
    .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Surname)
.NotEmpty().WithMessage("Enter a valid value")
.EmailAddress()
.WithMessage("A valid email address is required.");
            RuleFor(x => x.NationalIDNumber)
   .NotEmpty().WithMessage("Enter a valid value");

            RuleFor(x => x.ContactName)
  .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ContactSurname)
  .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.DateOfEstablishment).Must(BeWithinOneYear)
            .WithMessage("the business age may not be less than 1 year");
            RuleFor(x => x.Name)
  .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Surname)
  .NotEmpty().WithMessage("Enter a valid value");
        }

        private bool BeWithinOneYear(DateTime dateTime)
        {
            DateTime oneYearAgo = DateTime.Now.AddMonths(-1);
            return dateTime >= oneYearAgo && dateTime <= DateTime.Now;
        }
    }


}
