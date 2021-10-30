using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class UpdateSafeLocationValidator : IValidateRequest<UpdateSafeLocation>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public UpdateSafeLocationValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(UpdateSafeLocation request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var safeLocationExists = await _context.GroupsSafeLocations.AnyAsync(gsf => gsf.Id == request.SafeLocationId);

            if (!safeLocationExists)
            {
                return ValidationResult.GenericValidationError;
            }

            var isGroupAdmin = await _context.GroupsSafeLocations
                .AnyAsync(gsl => gsl.Group.Admin.Id == user.Id && gsl.Id == request.SafeLocationId);

            if (!isGroupAdmin)
            {
                return new DetailedValidationError(await _i18nProvider.Localize("cannot-update-safe-location"), await _i18nProvider.Localize("only-admin-cab-update-safe-location"));
            }

            return ValidationResult.Ok;
        }
    }
}