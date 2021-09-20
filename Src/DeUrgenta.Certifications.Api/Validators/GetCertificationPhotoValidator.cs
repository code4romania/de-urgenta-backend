using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.Validators
{
    public class GetCertificationPhotoValidator : IValidateRequest<GetCertificationPhoto>
    {
        private readonly DeUrgentaContext _context;

        public GetCertificationPhotoValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetCertificationPhoto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }

            var isOwner = await _context.Certifications.AnyAsync(c => c.UserId == user.Id
                                                                      && c.Id == request.CertificationId);
            if (!isOwner)
            {
                return false;
            }

            return true;
        }
    }
}
