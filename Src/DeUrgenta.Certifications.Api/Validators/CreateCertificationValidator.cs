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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
