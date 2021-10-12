using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
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

        public async Task<bool> IsValidAsync(AcceptInvite request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }

            var invite = await _context.Invites.FirstOrDefaultAsync(i => i.Id == request.InviteId);
            if (invite == null)
            {
                return false;
            }
            if (invite.InviteStatus == InviteStatus.Accepted)
            {
                return false;
            }

            return true;
        }
    }
}
