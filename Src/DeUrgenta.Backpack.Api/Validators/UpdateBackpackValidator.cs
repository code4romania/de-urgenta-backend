using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class UpdateBackpackValidator : IValidateRequest<UpdateBackpack>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public UpdateBackpackValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
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
                return new DetailedValidationError(await _i18nProvider.Localize("not-backpack-owner"), await _i18nProvider.Localize("only-backpack-owner-can-update-message"));
            }

            return ValidationResult.Ok;
        }
    }
}