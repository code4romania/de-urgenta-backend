using DeUrgenta.User.Api.Models.DTOs.Requests;
using FluentValidation;

namespace DeUrgenta.User.Api.Validators.RequestValidators
{
    public class UserResetPasswordRequestValidator : AbstractValidator<UserResetPasswordRequest>
    {
        public UserResetPasswordRequestValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().MinimumLength(1);
            RuleFor(c => c.ResetToken).NotEmpty().MinimumLength(1);
            RuleFor(c => c.NewPassword).NotEmpty().MinimumLength(1);
            RuleFor(c => c.NewPasswordConfirm).Equal(c => c.NewPassword);
        }
    }
}