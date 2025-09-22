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
    public class CreateSiteCommandValidator : AbstractValidator<CreateSiteCommand>
    {
        public CreateSiteCommandValidator(ISiteService sitesService)
        {
            RuleFor(request => request.SiteRequest.CustRegName)
              .NotEmpty()
                  .WithMessage("Site is required.");

            RuleFor(request => request.SiteRequest.PayTerms)
             .NotEmpty()
              .WithMessage("Payment Terms is required.");


            RuleFor(x => x).Must(x => !sitesService.IsDuplicateAsync(x.SiteRequest.CustRegName, x.SiteRequest.DistId).Result)
                .WithMessage("Siteal Distributor already exists.");
        }

    }
}