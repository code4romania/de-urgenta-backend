using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Options;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Group.Api.Validators
{
    public class AddSafeLocationValidator : IValidateRequest<AddSafeLocation>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;
        private readonly GroupsConfig _config;

        public AddSafeLocationValidator(DeUrgentaContext context, IamI18nProvider i18nProvider, IOptions<GroupsConfig> config)
        {
            _context = context;
            _i18nProvider = i18nProvider;
            _config = config.Value;
        }

        public async Task<ValidationResult> IsValidAsync(AddSafeLocation request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId);
            if (group == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isGroupAdmin = group.Admin.Id == user.Id;
            if (!isGroupAdmin)
            {
                return new DetailedValidationError(await _i18nProvider.Localize("cannot-add-safe-location"), await _i18nProvider.Localize("only-group-admin-can-add-locations-message"));
            }

            var groupHasMaxSafeLocations = group.GroupSafeLocations.Count >= _config.MaxSafeLocations;
            if (groupHasMaxSafeLocations)
            {
                return new DetailedValidationError(await _i18nProvider.Localize("group-safe-location-limit"), await _i18nProvider.Localize("group-safe-location-limit-message"));
            }

            return ValidationResult.Ok;
        }
    }
}