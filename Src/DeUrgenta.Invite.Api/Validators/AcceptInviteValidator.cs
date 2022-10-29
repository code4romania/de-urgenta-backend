using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Invite.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptInviteValidator : IValidateRequest<AcceptInvite>
    {
        private readonly DeUrgentaContext _context;

        public AcceptInviteValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(AcceptInvite request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var invite = await _context.Invites.FirstOrDefaultAsync(i => i.Id == request.InviteId, ct);
            if (invite == null)
            {
                return ValidationResult.GenericValidationError;
            }

            if (invite.InviteStatus == InviteStatus.Accepted)
            {
                return new LocalizableValidationError("cannot-accept-invite", "invite-already-accepted");
            }

            return ValidationResult.Ok;
        }
    }
}
