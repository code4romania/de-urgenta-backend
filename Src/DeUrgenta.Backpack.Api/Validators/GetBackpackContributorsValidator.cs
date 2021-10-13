using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class GetBackpackContributorsValidator : IValidateRequest<GetBackpackContributors>
    {
        private readonly DeUrgentaContext _context;

        public GetBackpackContributorsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetBackpackContributors request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isContributor = await _context.BackpacksToUsers.AnyAsync(btu => btu.User.Id == user.Id && btu.Backpack.Id == request.BackpackId);

            if (!isContributor)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}