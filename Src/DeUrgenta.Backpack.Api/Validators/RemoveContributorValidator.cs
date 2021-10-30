using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class RemoveContributorValidator : IValidateRequest<RemoveContributor>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public RemoveContributorValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(RemoveContributor request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null || targetUser == null)
            {
                return ValidationResult.GenericValidationError;
            }

            if (user.Id == request.UserId)
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
                return new DetailedValidationError(await _i18nProvider.Localize("not-backpack-owner"), await _i18nProvider.Localize("not-backpack-owner-delete-contributor-message"));
            }

            bool requestedUserIsContributor = await _context
                .BackpacksToUsers
                .AnyAsync(btu => btu.Backpack.Id == request.BackpackId && btu.User.Id == request.UserId);

            if (!requestedUserIsContributor)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}