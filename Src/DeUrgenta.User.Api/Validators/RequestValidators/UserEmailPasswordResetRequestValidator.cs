using DeUrgenta.User.Api.Models.DTOs.Requests;
using FluentValidation;

namespace DeUrgenta.User.Api.Validators.RequestValidators
{
    public class UserEmailPasswordResetRequestValidator : AbstractValidator<UserEmailPasswordResetRequest>
    {
        public UserEmailPasswordResetRequestValidator()
        {
            RuleFor(c => c.Email).NotEmpty().MinimumLength(1).EmailAddress();
        }
    }
}