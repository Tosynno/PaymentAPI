using FluentValidation;
using PaymentAPI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validations
{
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerRequestValidator()
        {
            RuleFor(x => x.NationalIDNumber).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Name)
    .NotEmpty().WithMessage("Enter a valid value");

            RuleFor(x => x.Surname)
  .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CustomerNumber)
  .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.DateofBirth)
         .NotNull()
         .Must(BeAtLeastEighteenYearsOld)
         .WithMessage("Must be at least eighteen years old.");

        }
        private static bool BeAtLeastEighteenYearsOld(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            return age >= 18;
        }
    }
}
