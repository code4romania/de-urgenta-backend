using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.User.Api.Queries;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.Validators
{
    public class GetUserLocationsValidator : IValidateRequest<GetUserLocations>
    {
        private readonly DeUrgentaContext _context;

        public GetUserLocationsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetUserLocations request, CancellationToken ct)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Sub == request.UserSub, ct);

            return userExists ? ValidationResult.Ok : ValidationResult.GenericValidationError;
        }
    }
}