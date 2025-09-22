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
    public class CreateRegionCommandValidator : AbstractValidator<CreateRegionCommand>
    {
        public CreateRegionCommandValidator(IRegionService regionsService)
        {
            RuleFor(request => request.RegionRequest.DistRegName)
              .NotEmpty()
                  .WithMessage("Regional Distributor is required.");

            RuleFor(request => request.RegionRequest.Region)
          .NotEmpty()
              .WithMessage("Region Name is required.");

            RuleFor(request => request.RegionRequest.PayTerms)
          .NotEmpty()
              .WithMessage("Payment Terms is required.");

            RuleFor(request => request.RegionRequest.Countries)
          .NotEmpty()
              .WithMessage("Country is required.");

            RuleFor(x => x).Must(x => !regionsService.IsDuplicateAsync(x.RegionRequest.DistRegName, x.RegionRequest.DistId).Result)
                .WithMessage("Regional Distributor already exists.");
        }

    }
}