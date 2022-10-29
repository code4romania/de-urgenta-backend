using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class GetMyBackpacksValidator : IValidateRequest<GetMyBackpacks>
    {
        private readonly DeUrgentaContext _context;

        public GetMyBackpacksValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetMyBackpacks request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}