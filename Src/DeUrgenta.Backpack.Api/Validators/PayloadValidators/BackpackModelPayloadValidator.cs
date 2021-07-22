using DeUrgenta.Backpack.Api.Models;
using FluentValidation;

namespace DeUrgenta.Backpack.Api.Validators.PayloadValidators
{
    public class BackpackModelPayloadValidator: AbstractValidator<BackpackModelRequest>
    {
        public BackpackModelPayloadValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
        }
    }
}
