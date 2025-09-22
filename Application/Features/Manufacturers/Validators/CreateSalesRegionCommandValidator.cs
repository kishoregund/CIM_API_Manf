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
    public class CreateSalesRegionCommandValidator : AbstractValidator<CreateSalesRegionCommand>
    {
        public CreateSalesRegionCommandValidator(ISalesRegionService salesRegionsService)
        {
            RuleFor(request => request.SalesRegionRequest.SalesRegionName)
              .NotEmpty()
                  .WithMessage("Sales Region Name is required.");

            RuleFor(request => request.SalesRegionRequest.PayTerms)
          .NotEmpty()
              .WithMessage("Payment Terms is required.");

            RuleFor(request => request.SalesRegionRequest.Countries)
          .NotEmpty()
              .WithMessage("Country is required.");

            RuleFor(x => x).Must(x => !salesRegionsService.IsDuplicateAsync(x.SalesRegionRequest.SalesRegionName, x.SalesRegionRequest.ManfId).Result)
                .WithMessage("Sales Region  already exists.");
        }

    }
}