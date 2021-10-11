using DeUrgenta.Invite.Api.Models;
using FluentValidation;

namespace DeUrgenta.Invite.Api.Validators.RequestValidators
{
    public class AcceptInviteRequestValidator : AbstractValidator<AcceptInviteRequest>
    {
        public AcceptInviteRequestValidator()
        {
            RuleFor(a => a.InviteId).NotEmpty();
        }
    }
}
