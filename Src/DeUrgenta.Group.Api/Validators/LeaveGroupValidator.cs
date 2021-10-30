using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class LeaveGroupValidator : IValidateRequest<LeaveGroup>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public LeaveGroupValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(LeaveGroup request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);

            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isAdmin = await _context.Groups.AnyAsync(g => g.Admin.Id == user.Id && g.Id == request.GroupId);
            if (isAdmin)
            {
                return new DetailedValidationError(await _i18nProvider.Localize("cannot-leave-group"), await _i18nProvider.Localize("cannot-leave-administered-group-message"));
            }

            var isPartOfGroup = await _context.UsersToGroups
                .AnyAsync(utg => utg.User.Id == user.Id && utg.Group.Id == request.GroupId);

            if (!isPartOfGroup)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}