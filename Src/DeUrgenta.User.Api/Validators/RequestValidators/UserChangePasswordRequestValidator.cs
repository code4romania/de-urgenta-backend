using DeUrgenta.User.Api.Models.DTOs.Requests;
using FluentValidation;

namespace DeUrgenta.User.Api.Validators.RequestValidators
{
    public class UserChangePasswordRequestValidator : AbstractValidator<UserChangePasswordRequest>
    {
        public UserChangePasswordRequestValidator()
        {
            RuleFor(c => c.ConfirmNewPassword).NotEmpty().MinimumLength(1);
            RuleFor(c => c.NewPassword).NotEmpty().MinimumLength(1);
            RuleFor(c => c.ConfirmNewPassword).Equal(c => c.NewPassword);
        }
    }
}