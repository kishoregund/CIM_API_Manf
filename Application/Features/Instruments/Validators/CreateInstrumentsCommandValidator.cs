using Application.Features.AppBasic.Commands;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Requests;
using Application.Features.Instruments.Commands;
using Application.Features.Schools;
using Application.Features.Schools.Commands;
using Application.Features.Schools.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Instruments.Validators
{
    public class CreateInstrumentCommandValidator : AbstractValidator<CreateInstrumentCommand>
    {
        public CreateInstrumentCommandValidator(IInstrumentService InstrumentService)
        {
            RuleFor(request => request.InstrumentRequest.SerialNos)
              .NotEmpty()
                  .WithMessage("Serial No. is required.");

            RuleFor(request => request.InstrumentRequest.InsMfgDt)
              .NotEmpty()
                  .WithMessage("Instrument Manufacturing Date is required.");

            RuleFor(request => request.InstrumentRequest.InsType)
              .NotEmpty()
                  .WithMessage("Instrument Type is required.");

            RuleFor(request => request.InstrumentRequest.InsVersion)
              .NotEmpty()
                  .WithMessage("Instrument Version is required.");


            RuleFor(x => x).Must(x => !InstrumentService.IsDuplicateAsync(x.InstrumentRequest.InsType, x.InstrumentRequest.SerialNos).Result)
                .WithMessage("Instrument already exists.");
        }

    }
}