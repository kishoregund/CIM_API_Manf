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
    public class CreateInstrumentAllocationCommandValidator : AbstractValidator<CreateInstrumentAllocationCommand>
    {
        public CreateInstrumentAllocationCommandValidator(IInstrumentAllocationService InstrumentAllocationService)
        {
            RuleFor(request => request.InstrumentAllocationRequest.InstrumentId)
              .NotEmpty()
                  .WithMessage("Instrument is required.");

            RuleFor(request => request.InstrumentAllocationRequest.DistributorId)
              .NotEmpty()
                  .WithMessage("Distributor is required.");

            RuleFor(request => request.InstrumentAllocationRequest.BusinessUnitId)
              .NotEmpty()
                  .WithMessage("Business Unit is required.");

            RuleFor(x => x).Must(x => !InstrumentAllocationService.IsDuplicateAsync(x.InstrumentAllocationRequest.InstrumentId, x.InstrumentAllocationRequest.DistributorId, x.InstrumentAllocationRequest.BusinessUnitId).Result)
                .WithMessage("Instrument Allocation already exists.");
        }

    }
}