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
    public class CreateDistributorCommandValidator : AbstractValidator<CreateDistributorCommand>
    {
        public CreateDistributorCommandValidator(IDistributorService distributorService)
        {
            RuleFor(request => request.DistributorRequest.DistName)
              .NotEmpty()
                  .WithMessage("Distributor Name is required.");

            RuleFor(request => request.DistributorRequest.Payterms)
              .NotEmpty()
                  .WithMessage("Payment Terms is required.");

            RuleFor(request => request.DistributorRequest.ManufacturerIds)
              .NotEmpty()
                  .WithMessage("Manufacturer is required.");

            RuleFor(x => x).Must(x => !distributorService.IsDuplicateAsync(x.DistributorRequest.DistName).Result)
                .WithMessage("Distributor already exists.");

            RuleFor(x => x.DistributorRequest.ManfBusinessUnitId)                
                .Must(x => distributorService.IsManfBURequired())
                .NotEmpty()
                .WithMessage("Manufacturer Business Unit is required.");

            
        }

    }
}