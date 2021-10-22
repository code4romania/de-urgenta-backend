using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class DeleteSafeLocationValidator : IValidateRequest<DeleteSafeLocation>
    {
        private readonly DeUrgentaContext _context;

        public DeleteSafeLocationValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(DeleteSafeLocation request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var safeLocationExists = await _context.GroupsSafeLocations.AnyAsync(gsl => gsl.Id == request.SafeLocationId);
            if (!safeLocationExists)
            {
                return ValidationResult.GenericValidationError;
            }

            var isGroupAdmin = await _context.GroupsSafeLocations
                .AnyAsync(gsl => gsl.Group.Admin.Id == user.Id && gsl.Id == request.SafeLocationId);

            if (!isGroupAdmin)
            {
                return new DetailedValidationError("Cannot delete safe locations", "Only group admins can delete safe locations.");
            }

            return ValidationResult.Ok;
        }
    }
}