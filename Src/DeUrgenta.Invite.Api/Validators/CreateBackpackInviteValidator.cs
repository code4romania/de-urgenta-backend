using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Invite.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Invite.Api.Validators
{
    public class CreateBackpackInviteValidator : ICreateInviteValidator
    {
        private DeUrgentaContext _context;

        public CreateBackpackInviteValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateAsync(CreateInvite request)
        {
            var backpack = await _context.Backpacks.FirstOrDefaultAsync(b => b.Id == request.DestinationId);
            if (backpack == null)
            {
                return false;
            }

            var userHasAccessToBackpack = await _context.BackpacksToUsers
                .AnyAsync(u => u.UserId == request.UserId
                                    && u.BackpackId == request.DestinationId);
            if (!userHasAccessToBackpack)
            {
                return false;
            }

            return true;
        }
    }
}