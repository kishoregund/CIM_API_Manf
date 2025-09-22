using Application.Features.AppBasic.Commands;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Requests;
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
    public class CreateManufacturerCommandValidator : AbstractValidator<CreateManufacturerCommand>
    {
        public CreateManufacturerCommandValidator(IManufacturerService manufacturerService)
        {
            RuleFor(request => request.ManufacturerRequest.ManfName)
              .NotEmpty()
                  .WithMessage("Manufacturer Name is required.");

            RuleFor(request => request.ManufacturerRequest.Payterms)
              .NotEmpty()
                  .WithMessage("Payment Terms is required.");
                        
            RuleFor(x => x).Must(x => !manufacturerService.IsDuplicateAsync(x.ManufacturerRequest.ManfName).Result)
                .WithMessage("Manufacturer already exists.");
        }

    }
}