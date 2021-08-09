using System.Threading.Tasks;
using DeUrgenta.Courses.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Courses.Api.Validators
{
    public class UpdateFirstAidCourseValidator : IValidateRequest<UpdateCourse>
    {
        private readonly DeUrgentaContext _context;
        public UpdateFirstAidCourseValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<bool> IsValidAsync(UpdateCourse request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }

            var isOwner = await _context.Certifications.AnyAsync(c => c.UserId == user.Id && c.Id == request.FirstAidCourseId);

            if (!isOwner)
            {
                return false;
            }

            return true;
        }
    }
}
