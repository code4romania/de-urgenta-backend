using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
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

        public async Task<ValidationResult> IsValidAsync(RemoveCurrentUserFromContributors request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isOwner = await _context
                .BackpacksToUsers
                .AnyAsync(btu => btu.User.Id == user.Id 
                                 && btu.Backpack.Id == request.BackpackId 
                                 && btu.IsOwner, ct);

            if (isOwner)
            {
                return new LocalizableValidationError("backpack-owner-leave", "backpack-owner-leave-message");
            }

            var isPartOfGroup = await _context
                .BackpacksToUsers
                .AnyAsync(btu => btu.User.Id == user.Id 
                                 && btu.Backpack.Id == request.BackpackId, ct);

            if (!isPartOfGroup)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}