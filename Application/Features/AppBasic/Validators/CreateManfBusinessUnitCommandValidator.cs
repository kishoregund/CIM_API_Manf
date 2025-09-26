using Application.Features.AppBasic.Commands;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Requests;
using Application.Features.Schools;
using Application.Features.Schools.Commands;
using Application.Features.Schools.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AppBasic.Validators
{
    public class CreateManfBusinessUnitCommandValidator : AbstractValidator<CreateManfBusinessUnitCommand>
    {
        public CreateManfBusinessUnitCommandValidator(IManfBusinessUnitService ManfBusinessUnitService)
        {
            RuleFor(request => request.Request.BusinessUnitName)
              .NotEmpty()
                  .WithMessage("Manufacturer BusinessUnitName is required.");
                        
            //RuleFor(x => x).Must(x => !ManfBusinessUnitService.IsDuplicateAsync(x.Request.ManfBusinessUnitName).Result)
            //    .WithMessage("Business Unit already exists.");
        }

    }
}