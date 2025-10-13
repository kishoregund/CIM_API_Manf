
using Application.Features.Instruments.Commands;
using FluentValidation;

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