using Application.Features.Customers.Commands;
using Application.Features.Customers.Requests;
using Application.Features.Schools;
using Application.Features.Schools.Commands;
using Application.Features.Schools.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Customers.Validators
{
    public class CreateCustomerInstrumentCommandValidator : AbstractValidator<CreateCustomerInstrumentCommand>
    {
        public CreateCustomerInstrumentCommandValidator(ICustInstrumentService custInstrumentService)
        {
            RuleFor(request => request.CustomerInstrumentRequest.CustSiteId)
              .NotEmpty()
                  .WithMessage("Customer is required.");

            RuleFor(request => request.CustomerInstrumentRequest.InstrumentId)
              .NotEmpty()
                  .WithMessage("Instrument is required.");

            RuleFor(request => request.CustomerInstrumentRequest.InsMfgDt)
             .NotEmpty()
                 .WithMessage("Manufacturing Date is required.");

            RuleFor(request => request.CustomerInstrumentRequest.InstallBy)
             .NotEmpty()
                 .WithMessage("Installed By Distributor is required.");

            RuleFor(request => request.CustomerInstrumentRequest.OperatorId)
             .NotEmpty()
                 .WithMessage("Operator Name  is required.");

            RuleFor(request => request.CustomerInstrumentRequest.InstruEngineerId)
             .NotEmpty()
                 .WithMessage("Instrumentation Engineer is required.");

            RuleFor(request => request.CustomerInstrumentRequest.InstallDt)
             .NotEmpty()
                 .WithMessage(" Instrument Installation Date is required.");

            When(x => x.CustomerInstrumentRequest.Warranty, () =>
            {
                RuleFor(x => x.CustomerInstrumentRequest.WrntyStDt).NotEmpty().WithMessage("Warranty Start Date is required");
                RuleFor(x => x.CustomerInstrumentRequest.WrntyEnDt).NotEmpty().WithMessage("Warranty End Date is required");
            });

            RuleFor(x => x).Must(x => !custInstrumentService.IsDuplicateAsync(x.CustomerInstrumentRequest).Result)
                .WithMessage("Instrument for this Customer Site already exists.");
        }

    }
}