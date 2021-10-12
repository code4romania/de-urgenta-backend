using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.Validators
{
    public class RejectGroupInviteValidator : IValidateRequest<RejectGroupInvite>
    {
        private readonly DeUrgentaContext _context;

        public RejectGroupInviteValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(RejectGroupInvite request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);

            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var inviteExists = await _context
                .GroupInvites
                .AnyAsync(bi => bi.InvitationReceiver.Sub == request.UserSub && bi.Id == request.GroupInviteId);

            return inviteExists ? ValidationResult.Ok : ValidationResult.GenericValidationError;
        }
    }
}