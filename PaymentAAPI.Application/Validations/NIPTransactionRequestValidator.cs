using FluentValidation;
using PaymentAPI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validations
{
    public class NIPTransactionRequestValidator : AbstractValidator<NIPTransactionRequest>
    {
        public NIPTransactionRequestValidator()
        {
            RuleFor(x => x.BankCode)
   .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.BankCode)
   .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.DebitAccountNumber)
    .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CreditMerchantNumber)
  .NotEmpty().WithMessage("Enter a valid value");


            RuleFor(x => x.TransactionAmount).GreaterThan(0).WithMessage("Enter a valid value");
        }
    }
}
