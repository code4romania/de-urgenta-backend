using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
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

        public async Task<ValidationResult> IsValidAsync(RemoveContributor request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);
            var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

            if (user == null || targetUser == null)
            {
                return ValidationResult.GenericValidationError;
            }

            if (user.Id == request.UserId)
            {
                return ValidationResult.GenericValidationError;
            }

            var backpackExists = await _context.Backpacks.AnyAsync(b => b.Id == request.BackpackId, ct);
            if (!backpackExists)
            {
                return ValidationResult.GenericValidationError;
            }

            var isOwner = await _context.BackpacksToUsers.AnyAsync(btu => btu.User.Id == user.Id 
                                                                          && btu.Backpack.Id == request.BackpackId 
                                                                          && btu.IsOwner, ct);

            if (!isOwner)
            {
                return new LocalizableValidationError("not-backpack-owner", "not-backpack-owner-delete-contributor-message");
            }

            var requestedUserIsContributor = await _context
                .BackpacksToUsers
                .AnyAsync(btu => btu.Backpack.Id == request.BackpackId 
                                 && btu.User.Id == request.UserId, ct);

            if (!requestedUserIsContributor)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}