using System;
using DeUrgenta.FirstAidCourse.Api.Models;
using FluentValidation;

namespace DeUrgenta.FirstAidCourse.Api.Validators.RequestValidators
{
    public class FirstAidCourseRequestValidator : AbstractValidator<FirstAidCourseRequest>
    {
        public FirstAidCourseRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.IssuingAuthority).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.ExpirationDate).GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}
