using System;
using DeUrgenta.Courses.Api.Models;
using FluentValidation;

namespace DeUrgenta.Courses.Api.Validators.RequestValidators
{
    public class CourseRequestValidator : AbstractValidator<CourseRequest>
    {
        public CourseRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.IssuingAuthority).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.ExpirationDate).GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}
