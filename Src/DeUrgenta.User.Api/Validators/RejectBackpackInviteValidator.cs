using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.Validators
{
    public class RejectBackpackInviteValidator : IValidateRequest<RejectBackpackInvite>
    {
        private readonly DeUrgentaContext _context;

        public RejectBackpackInviteValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(RejectBackpackInvite request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);

            if (user == null)
            {
                return false;
            }

            var inviteExists = await _context
                .BackpackInvites
                .AnyAsync(bi => bi.InvitationReceiver.Sub == request.UserSub && bi.Id == request.BackpackInviteId);

            return inviteExists;
        }
    }
}