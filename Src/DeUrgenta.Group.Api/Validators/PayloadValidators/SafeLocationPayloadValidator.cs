using DeUrgenta.Group.Api.Models;
using FluentValidation;

namespace DeUrgenta.Group.Api.Validators.PayloadValidators
{
    public class SafeLocationPayloadValidator : AbstractValidator<SafeLocationRequest>
    {
        public SafeLocationPayloadValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        }
    }
}
