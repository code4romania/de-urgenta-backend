using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.Validators
{
    public class DeleteCertificationValidator : IValidateRequest<DeleteCertification>
    {
        private readonly DeUrgentaContext _context;

        public DeleteCertificationValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(DeleteCertification request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var certificationExist = await _context.Certifications.AnyAsync(c => c.UserId == user.Id && c.Id == request.CertificationId);

            if (!certificationExist)
            {
                return new DetailedValidationError("Certification does not exists", $"Requested certification could not be found.");
            }

            return ValidationResult.Ok;
        }
    }
}
