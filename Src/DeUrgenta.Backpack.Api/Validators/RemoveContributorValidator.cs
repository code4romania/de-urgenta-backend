using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class RemoveContributorValidator : IValidateRequest<RemoveContributor>
    {
        private readonly DeUrgentaContext _context;

        public RemoveContributorValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(RemoveContributor request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null || targetUser == null)
            {
                return false;
            }

            if (user.Id == request.UserId)
            {
                return false;
            }

            var isOwner = await _context.BackpacksToUsers.AnyAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId && btu.IsOwner);

            if (!isOwner)
            {
                return false;
            }

            bool requestedUserIsContributor = await _context
                .BackpacksToUsers
                .AnyAsync(btu => btu.Backpack.Id == request.BackpackId && btu.User.Id == request.UserId);

            if (!requestedUserIsContributor)
            {
                return false;
            }

            return true;
        }
    }
}