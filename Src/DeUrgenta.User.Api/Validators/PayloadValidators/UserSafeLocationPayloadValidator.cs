using DeUrgenta.User.Api.Models;
using FluentValidation;

namespace DeUrgenta.User.Api.Validators.PayloadValidators
{
    public class UserSafeLocationPayloadValidator : AbstractValidator<UserSafeLocationRequest>
    {
        public UserSafeLocationPayloadValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        }
    }
}
