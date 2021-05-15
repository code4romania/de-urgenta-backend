using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class GetBackpackCategoryItemsValidator : IValidateRequest<GetBackpackCategoryItems>
    {
        private readonly DeUrgentaContext _context;

        public GetBackpackCategoryItemsValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<bool> IsValidAsync(GetBackpackCategoryItems request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }

            var isContributor = await _context.BackpacksToUsers.AnyAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId);
            if (!isContributor)
            {
                return false;
            }

            return await _context.Backpacks.AnyAsync(x => x.Id == request.BackpackId);
        }
    }
}
