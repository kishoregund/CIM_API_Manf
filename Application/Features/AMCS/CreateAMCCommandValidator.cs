using Application.Features.Customers.Commands;
using Application.Features.Customers;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.AMCS.Commands;

namespace Application.Features.AMCS
{
    public class CreateAMCCommandValidator : AbstractValidator<CreateAmcCommand>
    {
        public CreateAMCCommandValidator(IAmcService amcService)
        {
            RuleFor(request => request.CreateAmcRequest.BillTo)
              .NotEmpty()
                  .WithMessage("Customer is required.");

            RuleFor(request => request.CreateAmcRequest.CustSite)
              .NotEmpty()
                  .WithMessage("Customer Site is required.");

            RuleFor(request => request.CreateAmcRequest.ServiceQuote)
             .NotEmpty()
                 .WithMessage("Service Quote is required.");

            RuleFor(request => request.CreateAmcRequest.SqDate)
             .NotEmpty()
                 .WithMessage("SQ Date is required.");

            RuleFor(request => request.CreateAmcRequest.SDate)
             .NotEmpty()
                 .WithMessage("Start Date is required.");

            RuleFor(request => request.CreateAmcRequest.EDate)
             .NotEmpty()
                 .WithMessage("End Date is required.");

            RuleFor(x => x).Must(x => !amcService.ServiceQuoteExists(x.CreateAmcRequest.ServiceQuote).Result)
                .WithMessage("Service Quote already exists.");
        }

    }
}