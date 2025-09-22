using Application.Features.Distributors.Commands;
using Application.Features.Distributors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ServiceReports.Commands;

namespace Application.Features.ServiceReports.Validators
{
    public class CreateWorkDoneCommandValidator : AbstractValidator<CreateSRPEngWorkDoneCommand>
    {
        public CreateWorkDoneCommandValidator(ISRPEngWorkDoneService srpEngWorkDoneService)
        {
            RuleFor(request => request.SRPEngWorkDoneRequest.Workdone)
              .NotEmpty()
                  .WithMessage("Work Done is required.");


            RuleFor(request => request.SRPEngWorkDoneRequest.Workdone)
              .Length(1, 500)
                  .WithMessage("Work Done max length is 500 characters.");

        }
    }
}
