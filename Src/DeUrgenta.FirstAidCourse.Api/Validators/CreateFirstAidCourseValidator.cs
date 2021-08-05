using System.Threading.Tasks;
using DeUrgenta.FirstAidCourse.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.FirstAidCourse.Api.Validators
{
    public class CreateFirstAidCourseValidator : IValidateRequest<CreateFirstAidCourse>
    {
        private readonly DeUrgentaContext _context;

        public CreateFirstAidCourseValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(CreateFirstAidCourse request)
        {
            var isExistingUser = await _context.Users.AnyAsync(u => u.Sub == request.UserSub);

            return isExistingUser;
        }
    }
}
