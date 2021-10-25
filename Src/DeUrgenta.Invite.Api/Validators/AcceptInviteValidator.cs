using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Invite.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptInviteValidator : IValidateRequest<AcceptInvite>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public AcceptInviteValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }

        public async Task<ValidationResult> IsValidAsync(AcceptInvite request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var invite = await _context.Invites.FirstOrDefaultAsync(i => i.Id == request.InviteId);
            if (invite == null)
            {
                return ValidationResult.GenericValidationError;
            }

            if (invite.InviteStatus == InviteStatus.Accepted)
            {
                return new DetailedValidationError("Cannot accept invite", "Invite was already accepted.");
            }

            return ValidationResult.Ok;
        }
    }
}
