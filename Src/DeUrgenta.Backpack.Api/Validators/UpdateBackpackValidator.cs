using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class UpdateBackpackValidator : IValidateRequest<UpdateBackpack>
    {
        private readonly DeUrgentaContext _context;

        public UpdateBackpackValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(UpdateBackpack request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var backpackToUser = await _context
                .BackpacksToUsers
                .FirstOrDefaultAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId);

            if (backpackToUser == null)
            {
                return ValidationResult.GenericValidationError;
            }

            if (!backpackToUser.IsOwner)
            {
                return new LocalizableValidationError("not-backpack-owner", "only-backpack-owner-can-update-message");
            }

            return ValidationResult.Ok;
        }
    }
}