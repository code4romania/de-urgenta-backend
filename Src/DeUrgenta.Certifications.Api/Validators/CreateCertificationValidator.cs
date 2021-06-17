using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
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

        public async Task<bool> IsValidAsync(CreateCertification request)
        {
            var isExistingUser = await _context.Users.AnyAsync(u => u.Sub == request.UserSub);

            return isExistingUser;
        }
    }
}
