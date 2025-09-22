using Application.Features.AppBasic.Commands;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Requests;
using Application.Features.Distributors.Commands;
using Application.Features.Manufacturers;
using Application.Features.Manufacturers.Commands;
using Application.Features.Schools;
using Application.Features.Schools.Commands;
using Application.Features.Schools.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Manufacturers.Validators
{
    public class CreateSalesRegionContactCommandValidator : AbstractValidator<CreateSalesRegionContactCommand>
    {
        public CreateSalesRegionContactCommandValidator(ISalesRegionContactService SalesRegionContactsService)
        {
            RuleFor(request => request.SalesRegionContactRequest.FirstName)
              .NotEmpty()
                  .WithMessage("First Name is required.");

            RuleFor(request => request.SalesRegionContactRequest.LastName)
          .NotEmpty()
              .WithMessage("Last Name is required.");

            RuleFor(request => request.SalesRegionContactRequest.PrimaryEmail)
          .NotEmpty()
              .WithMessage("Primary Email is required.");

            RuleFor(request => request.SalesRegionContactRequest.PrimaryContactNo)
          .NotEmpty()
              .WithMessage("Primary Contact No. is required.");

            RuleFor(x => x).Must(x => !SalesRegionContactsService.IsDuplicateAsync(x.SalesRegionContactRequest).Result)
                .WithMessage("Contact already exists.");
        }

    }
}