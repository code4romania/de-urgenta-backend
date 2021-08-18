using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class RemoveCurrentUserFromContributorsValidator : IValidateRequest<RemoveCurrentUserFromContributors>
    {
        private readonly DeUrgentaContext _context;

        public RemoveCurrentUserFromContributorsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(RemoveCurrentUserFromContributors request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }

            var isOwner = await _context
                .BackpacksToUsers
                .AnyAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId && btu.IsOwner);

            if (isOwner)
            {
                return false;
            }

            var isPartOfGroup = await _context
                .BackpacksToUsers
                .AnyAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId);

            if (!isPartOfGroup)
            {
                return false;
            }

            return true;
        }
    }
}