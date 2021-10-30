using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class RemoveCurrentUserFromContributorsValidator : IValidateRequest<RemoveCurrentUserFromContributors>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public RemoveCurrentUserFromContributorsValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(RemoveCurrentUserFromContributors request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isOwner = await _context
                .BackpacksToUsers
                .AnyAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId && btu.IsOwner);

            if (isOwner)
            {
                return new DetailedValidationError(await _i18nProvider.Localize("backpack-owner-leave"), await _i18nProvider.Localize("backpack-owner-leave-message"));
            }

            var isPartOfGroup = await _context
                .BackpacksToUsers
                .AnyAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId);

            if (!isPartOfGroup)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}