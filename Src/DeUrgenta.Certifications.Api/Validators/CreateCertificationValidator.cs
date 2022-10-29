using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.Validators
{
    public class CreateCertificationValidator : IValidateRequest<CreateCertification>
    {
        private readonly DeUrgentaContext _context;

        public CreateCertificationValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(CreateCertification request, CancellationToken ct)
        {
            var isExistingUser = await _context.Users.AnyAsync(u => u.Sub == request.UserSub, ct);

            return isExistingUser ? ValidationResult.Ok : ValidationResult.GenericValidationError;
        }
    }
}
