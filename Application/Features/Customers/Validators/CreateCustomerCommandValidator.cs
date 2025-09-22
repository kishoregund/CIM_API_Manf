using Application.Features.AppBasic.Commands;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Requests;
using Application.Features.Customers.Commands;
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
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator(ICustomerService customerService)
        {
            RuleFor(request => request.CustomerRequest.CustName)
              .NotEmpty()
                  .WithMessage("Customer Name is required.");

            RuleFor(request => request.CustomerRequest.DefDistId)
              .NotEmpty()
                  .WithMessage("Distributor is required.");

            RuleFor(request => request.CustomerRequest.DefDistRegionId)
              .NotEmpty()
                  .WithMessage("Default Distributor Region is required.");

            RuleFor(x => x).Must(x => !customerService.IsDuplicateAsync(x.CustomerRequest.CustName).Result)
                .WithMessage("Customer already exists.");
        }

    }
}