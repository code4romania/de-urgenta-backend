using DeUrgenta.User.Api.Models;
using FluentValidation;

namespace DeUrgenta.User.Api.Validators.RequestValidators
{
    public class UserSafeLocationRequestValidator : AbstractValidator<UserLocationRequest>
    {
        public UserSafeLocationRequestValidator()
        {
            RuleFor(c => c.Address).NotEmpty().MinimumLength(3).MaximumLength(100);
        }
    }
}
