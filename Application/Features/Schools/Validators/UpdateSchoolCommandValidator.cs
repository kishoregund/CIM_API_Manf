﻿using Application.Features.Schools.Commands;
using FluentValidation;

namespace Application.Features.Schools.Validators
{
    public class UpdateSchoolCommandValidator : AbstractValidator<UpdateSchoolCommand>
    {
        public UpdateSchoolCommandValidator(ISchoolService schoolService)
        {
            RuleFor(command => command.SchoolRequest)
                .SetValidator(new UpdateSchoolRequestValidator(schoolService));
        }
    }
}
