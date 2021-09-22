using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class UpdateBackpackItemValidator : IValidateRequest<UpdateBackpackItem>
    {
        private readonly DeUrgentaContext _context;
        public UpdateBackpackItemValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<bool> IsValidAsync(UpdateBackpackItem request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }

            var backpackItem = await _context.BackpackItems.FirstOrDefaultAsync(x => x.Id == request.ItemId);
            if (backpackItem == null)
            {
                return false;
            }

            var backpack = await _context.Backpacks.FirstOrDefaultAsync(b => b.Id == backpackItem.BackpackId);
            
            var isContributor = await _context.BackpacksToUsers.AnyAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == backpack.Id);
            if (!isContributor)
            {
                return false;
            }

            return true;
        }
    }
}
