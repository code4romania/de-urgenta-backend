using DeUrgenta.Group.Api.Models;
using FluentValidation;

namespace DeUrgenta.Group.Api.Validators.RequestValidators
{
    public class SafeLocationRequestValidator : AbstractValidator<SafeLocationRequest>
    {
        public SafeLocationRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        }
    }
}
