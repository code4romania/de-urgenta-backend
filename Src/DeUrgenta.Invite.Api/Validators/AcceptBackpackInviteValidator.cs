using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptBackpackInviteValidator : IValidateRequest<AcceptBackpackInvite>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;
        private readonly BackpacksConfig _config;

        public AcceptBackpackInviteValidator(DeUrgentaContext context, IamI18nProvider i18nProvider, IOptions<BackpacksConfig> config)
        {
            _context = context;
            _i18nProvider = i18nProvider;
            _config = config.Value;
        }

        public async Task<ValidationResult> IsValidAsync(AcceptBackpackInvite request)
        {
            var backpack = await _context.Backpacks.FirstOrDefaultAsync(b => b.Id == request.BackpackId);
            if (backpack == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub);

            var userIsAlreadyAContributor = await _context.BackpacksToUsers
                .AnyAsync(u => u.UserId == user.Id && u.BackpackId == request.BackpackId);

            if (userIsAlreadyAContributor)
            {
                return new DetailedValidationError("Cannot accept invite", "User is already a backpack contributor.");
            }

            var existingContributors = await _context.BackpacksToUsers.CountAsync(b => b.BackpackId == request.BackpackId);
            if (existingContributors >= _config.MaxContributors)
            {
                return new DetailedValidationError("Cannot accept invite", "Current maximum number of contributors is reached.");
            }

            return ValidationResult.Ok;
        }

    }
}