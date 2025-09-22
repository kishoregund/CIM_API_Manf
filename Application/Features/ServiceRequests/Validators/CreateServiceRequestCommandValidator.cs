using Application.Features.Instruments.Commands;
using Application.Features.Instruments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.ServiceRequests.Commands;
using Application.Features.Identity.Users;

namespace Application.Features.ServiceRequests.Validators
{
    internal class CreateServiceRequestCommandValidator : AbstractValidator<CreateServiceRequestCommand>
    {
        public CreateServiceRequestCommandValidator(IServiceRequestService serviceRequestService, ICurrentUserService currentUserService)
        {
            RuleFor(request => request.ServiceRequestRequest.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .Unless(x => string.IsNullOrEmpty(x.ServiceRequestRequest.Email));
                      
            RuleFor(x => x).Must(x => !serviceRequestService.OnBehalfOfCheck(Guid.Parse(currentUserService.GetUserId())).Result)
               .WithMessage("On Behalf-Of is required.");
        }
    }
}
