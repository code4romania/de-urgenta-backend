using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
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

        public async Task<ValidationResult> IsValidAsync(UpdateSafeLocation request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isGroupAdmin = await _context.GroupsSafeLocations
                .AnyAsync(gsl => gsl.Group.Admin.Id == user.Id && gsl.Id == request.SafeLocationId);

            if (!isGroupAdmin)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}