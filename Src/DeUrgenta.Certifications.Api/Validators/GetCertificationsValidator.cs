using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.Validators
{
    public class GetCertificationsValidator : IValidateRequest<GetCertifications>
    {
        private readonly DeUrgentaContext _context;

        public GetCertificationsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetCertifications request, CancellationToken ct)
        {
            var isExistingUser = await _context.Users.AnyAsync(u => u.Sub == request.UserSub, ct);

            return isExistingUser ? ValidationResult.Ok : ValidationResult.GenericValidationError;
        }
    }
}
