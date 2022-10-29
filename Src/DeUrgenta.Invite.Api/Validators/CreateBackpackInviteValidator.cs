using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Invite.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Invite.Api.Validators
{
    public class CreateBackpackInviteValidator : ICreateInviteValidator
    {
        private readonly DeUrgentaContext _context;

        public CreateBackpackInviteValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> ValidateAsync(CreateInvite request, CancellationToken ct)
        {
            var backpack = await _context.Backpacks.FirstOrDefaultAsync(b => b.Id == request.DestinationId, ct);
            if (backpack == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var userHasAccessToBackpack = await _context.BackpacksToUsers
                .AnyAsync(u => u.UserId == request.UserId && u.BackpackId == request.DestinationId, ct);

            if (!userHasAccessToBackpack)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}