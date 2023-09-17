using FluentValidation;
using PaymentAPI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validations
{
    public class IntraBankTransferRequestValidator : AbstractValidator<IntraBankTransferRequest>
    {
        public IntraBankTransferRequestValidator()
        {

            RuleFor(x => x.DebitMerchantNumber)
    .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CreditMerchantNumber)
  .NotEmpty().WithMessage("Enter a valid value");


            RuleFor(x => x.TransactionAmount).GreaterThan(0).WithMessage("Enter a valid value");
        }
    }
}
