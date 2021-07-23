using DeUrgenta.User.Api.Models;
using FluentValidation;

namespace DeUrgenta.User.Api.Validators.RequestValidators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().MinimumLength(1).MaximumLength(100);
            RuleFor(c => c.LastName).NotEmpty().MinimumLength(1).MaximumLength(100);
        }
    }
}
