using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.Validators
{
    public class UpdateCertificationValidator : IValidateRequest<UpdateCertification>
    {
        private readonly DeUrgentaContext _context;
        public UpdateCertificationValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<ValidationResult> IsValidAsync(UpdateCertification request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isOwner = await _context.Certifications.AnyAsync(c => c.UserId == user.Id && c.Id == request.CertificationId);

            if (!isOwner)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}
