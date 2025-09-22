using Application.Features.ServiceReports.Commands;
using Application.Features.ServiceReports;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Masters.Commands;

namespace Application.Features.Masters.Validators
{
    internal class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
    {
        public CreateCurrencyCommandValidator(ICurrencyService currencyService)
        {
            RuleFor(request => request.CurrencyRequest.Symbol)
              .NotEmpty()
                  .WithMessage("Symbol is required.");


            RuleFor(request => request.CurrencyRequest.Symbol)
              .Length(1, 2)
                  .WithMessage("Symbol max length is 2 characters.");

        }
    }
}
