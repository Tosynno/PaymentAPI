using FluentValidation;
using PaymentAPI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Validations
{
    public class AverageTransactionRequestValidator : AbstractValidator<AverageTransactionRequest>
    {
        public AverageTransactionRequestValidator()
        {

            RuleFor(x => x.BusinessId)
    .NotEmpty().WithMessage("Enter a valid value");

            RuleFor(x => x.TransactionAmountlimit).GreaterThan(0).WithMessage("Enter a valid value");
        }
    }
}
