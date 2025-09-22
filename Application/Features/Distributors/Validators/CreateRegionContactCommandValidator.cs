using Application.Features.AppBasic.Commands;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Requests;
using Application.Features.Distributors.Commands;
using Application.Features.Schools;
using Application.Features.Schools.Commands;
using Application.Features.Schools.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Distributors.Validators
{
    public class CreateRegionContactCommandValidator : AbstractValidator<CreateRegionContactCommand>
    {
        public CreateRegionContactCommandValidator(IRegionContactService regionContactsService)
        {
            RuleFor(request => request.RegionContactRequest.FirstName)
              .NotEmpty()
                  .WithMessage("First Name is required.");

            RuleFor(request => request.RegionContactRequest.LastName)
          .NotEmpty()
              .WithMessage("Last Name is required.");

            RuleFor(request => request.RegionContactRequest.PrimaryEmail)
          .NotEmpty()
              .WithMessage("Primary Email is required.");

            RuleFor(request => request.RegionContactRequest.PrimaryContactNo)
          .NotEmpty()
              .WithMessage("Primary Contact No. is required.");

            RuleFor(request => request.RegionContactRequest.AddrCountryId)
            .NotEmpty()
            .WithMessage("Address Country is required.");

            RuleFor(x => x).Must(x => !regionContactsService.IsDuplicateAsync(x.RegionContactRequest).Result)
                .WithMessage("Contact already exists.");
        }

    }
}