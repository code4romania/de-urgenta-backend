using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.User.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.Validators
{
    public class UpdateLocationValidator : IValidateRequest<UpdateLocation>
    {
        private readonly DeUrgentaContext _context;

        public UpdateLocationValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(UpdateLocation request, CancellationToken ct)
        {
            var locationExists = await _context.Users.AnyAsync(u => u.Sub == request.UserSub
                                                                    && u.Locations.Any(l => l.Id == request.LocationId),
                ct);

            return locationExists ? ValidationResult.Ok : ValidationResult.GenericValidationError;
        }
    }
}