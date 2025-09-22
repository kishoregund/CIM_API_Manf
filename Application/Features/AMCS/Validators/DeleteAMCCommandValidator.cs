using Application.Features.AMCS.Commands;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.AMCS.Validators
{
    public class DeleteAmcCommandValidator : AbstractValidator<DeleteAmcCommand>
    {
        public DeleteAmcCommandValidator(IAmcService amcService)
        {
            //RuleFor(command => command.Id)
            //    .NotEmpty()
            //    .MustAsync(async (id, ct) => await AMCService.GetAMCByIdAsync(id) is AMC AMCInDb && AMCInDb.Id == id)
            //        .WithMessage("AMC does not exists.");

            //RuleFor(x => x.Id).MustAsync(async(id,ct) => await !amcService.CheckDeleteAmc(x.Id).Result)
            //    .WithMessage("AMC cannot be delete as it has mutliple child data associated.");
        }
    }
}
