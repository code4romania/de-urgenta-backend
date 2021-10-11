using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Invite.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptBackpackInviteValidator : IAcceptInviteValidator
    {
        private readonly DeUrgentaContext _context;

        public AcceptBackpackInviteValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateAsync(AcceptInvite request)
        {
            var backpack = await _context.Backpacks.FirstOrDefaultAsync(b => b.Id == request.DestinationId);
            if (backpack == null)
            {
                return false;
            }

            var userIsAlreadyAContributor = await _context.BackpacksToUsers
                .AnyAsync(u => u.UserId == request.UserId
                                    && u.BackpackId == request.DestinationId);
            if (userIsAlreadyAContributor)
            {
                return false;
            }

            return true;
        }
    }
}