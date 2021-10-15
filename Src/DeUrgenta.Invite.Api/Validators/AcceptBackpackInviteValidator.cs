using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Invite.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptBackpackInviteValidator : IValidateRequest<AcceptBackpackInvite>
    {
        private readonly DeUrgentaContext _context;

        public AcceptBackpackInviteValidator(DeUrgentaContext context)
        {
            _context = context;
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
                .AnyAsync(u => u.UserId == user.Id
                                    && u.BackpackId == request.BackpackId);
            if (userIsAlreadyAContributor)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }

    }
}