using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class UpdateSafeLocationValidator : IValidateRequest<UpdateSafeLocation>
    {
        private readonly DeUrgentaContext _context;

        public UpdateSafeLocationValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(UpdateSafeLocation request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var safeLocationExists = await _context.GroupsSafeLocations.AnyAsync(gsf => gsf.Id == request.SafeLocationId, ct);

            if (!safeLocationExists)
            {
                return ValidationResult.GenericValidationError;
            }

            var isGroupAdmin = await _context.GroupsSafeLocations
                .AnyAsync(gsl => gsl.Group.Admin.Id == user.Id && gsl.Id == request.SafeLocationId, ct);

            if (!isGroupAdmin)
            {
                return new LocalizableValidationError("cannot-update-safe-location", "only-admin-cab-update-safe-location");
            }

            return ValidationResult.Ok;
        }
    }
}