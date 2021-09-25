using System;
using DeUrgenta.Backpack.Api.Models;
using FluentValidation;

namespace DeUrgenta.Backpack.Api.Validators.RequestValidators
{
    public class BackpackItemRequestValidator : AbstractValidator<BackpackItemRequest>
    {
        public BackpackItemRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => (int)c.Amount).InclusiveBetween(1, 99999);
            RuleFor(c => c.ExpirationDate).GreaterThanOrEqualTo(DateTime.Today).When(c => c.ExpirationDate.HasValue);
            RuleFor(c => c.CategoryType).IsInEnum();
        }
    }
}
