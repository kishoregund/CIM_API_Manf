using Application.Features.AppBasic.Commands;
using Application.Features.Customers;
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

namespace Application.Features.Customers.Validators
{
    public class CreateSiteContactCommandValidator : AbstractValidator<CreateSiteContactCommand>
    {
        public CreateSiteContactCommandValidator(ISiteContactService siteContactsService)
        {
            RuleFor(request => request.SiteContactRequest.FirstName)
              .NotEmpty()
                  .WithMessage("First Name is required.");

            RuleFor(request => request.SiteContactRequest.LastName)
          .NotEmpty()
              .WithMessage("Last Name is required.");

            RuleFor(request => request.SiteContactRequest.PrimaryEmail)
          .NotEmpty()
              .WithMessage("Primary Email is required.");

            RuleFor(request => request.SiteContactRequest.PrimaryContactNo)
          .NotEmpty()
              .WithMessage("Primary Contact No. is required.");

            RuleFor(x => x).Must(x => !siteContactsService.IsDuplicateAsync(x.SiteContactRequest).Result)
                .WithMessage("Contact already exists.");
        }

    }
}