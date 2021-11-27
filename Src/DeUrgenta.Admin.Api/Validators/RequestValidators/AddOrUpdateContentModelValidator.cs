using DeUrgenta.Admin.Api.Models;
using FluentValidation;

namespace DeUrgenta.Admin.Api.Validators.RequestValidators
{
    public class AddOrUpdateContentModelValidator : AbstractValidator<AddOrUpdateContentModel>
    {
        public AddOrUpdateContentModelValidator()
        {
            RuleFor(c => c.Culture).NotEmpty().MaximumLength(250);
            RuleFor(c => c.Key).NotEmpty().MaximumLength(250);
            RuleFor(c => c.Value).NotEmpty();
        }
    }
}