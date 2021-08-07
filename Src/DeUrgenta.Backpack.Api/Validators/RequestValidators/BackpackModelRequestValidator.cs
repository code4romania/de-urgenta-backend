using DeUrgenta.Backpack.Api.Models;
using FluentValidation;

namespace DeUrgenta.Backpack.Api.Validators.RequestValidators
{
    public class BackpackModelRequestValidator : AbstractValidator<BackpackModelRequest>
    {
        public BackpackModelRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
        }
    }
}
