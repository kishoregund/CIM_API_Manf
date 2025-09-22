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
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator(IBrandService brandService)
        {
            RuleFor(request => request.Request.BrandName)
              .NotEmpty()
                  .WithMessage("Brand Name is required.");
                        
            RuleFor(x => x).Must(x => !brandService.IsDuplicateAsync(x.Request.BrandName, x.Request.BusinessUnitId).Result)
                .WithMessage("Brand already exists.");
        }

    }
}