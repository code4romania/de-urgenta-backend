using DeUrgenta.Invite.Api.Models;
using FluentValidation;

namespace DeUrgenta.Invite.Api.Validators.RequestValidators
{
    public class InviteRequestValidator : AbstractValidator<InviteRequest>
    {
        public InviteRequestValidator()
        {
            RuleFor(r => r.DestinationId).NotEmpty();
            RuleFor(r => r.Type).IsInEnum();
        }
    }
}
