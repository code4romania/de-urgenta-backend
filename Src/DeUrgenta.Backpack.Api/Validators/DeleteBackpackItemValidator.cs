using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class DeleteBackpackItemValidator : IValidateRequest<DeleteBackpackItem>
    {
        private readonly DeUrgentaContext _context;
        public DeleteBackpackItemValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(DeleteBackpackItem request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var backpackItem = await _context.BackpackItems.FirstOrDefaultAsync(x => x.Id == request.ItemId, ct);
            if (backpackItem == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var backpack = await _context.Backpacks.FirstOrDefaultAsync(b => b.Id == backpackItem.BackpackId, ct);

            var isContributor = await _context.BackpacksToUsers.AnyAsync(btu => btu.User.Id == user.Id 
                && btu.Backpack.Id == backpack.Id, ct);
            if (!isContributor)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}
