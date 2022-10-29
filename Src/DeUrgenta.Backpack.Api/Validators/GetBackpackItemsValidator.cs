using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class GetBackpackItemsValidator : IValidateRequest<GetBackpackItems>
    {
        private readonly DeUrgentaContext _context;

        public GetBackpackItemsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetBackpackItems request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isContributor = await _context.BackpacksToUsers.AnyAsync(btu => btu.User.Id == user.Id 
                && btu.Backpack.Id == request.BackpackId, ct);
            if (!isContributor)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}
