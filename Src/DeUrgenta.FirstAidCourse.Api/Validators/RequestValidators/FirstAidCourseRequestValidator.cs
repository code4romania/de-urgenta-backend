using System;
using DeUrgenta.Courses.Api.Models;
using FluentValidation;

namespace DeUrgenta.Courses.Api.Validators.RequestValidators
{
    public class FirstAidCourseRequestValidator : AbstractValidator<CourseRequest>
    {
        public FirstAidCourseRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.IssuingAuthority).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.ExpirationDate).GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}
