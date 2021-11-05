using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class DeleteBackpackValidator : IValidateRequest<DeleteBackpack>
    {
        private readonly DeUrgentaContext _context;

        public DeleteBackpackValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(DeleteBackpack request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var backpackExists = await _context.Backpacks.AnyAsync(b => b.Id == request.BackpackId);
            if (!backpackExists)
            {
                return ValidationResult.GenericValidationError;
            }

            var isOwner = await _context.BackpacksToUsers.AnyAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId && btu.IsOwner);

            if (!isOwner)
            {
                return new LocalizableValidationError("not-backpack-owner", "not-backpack-owner-delete-message");
            }

            return ValidationResult.Ok;
        }
    }
}