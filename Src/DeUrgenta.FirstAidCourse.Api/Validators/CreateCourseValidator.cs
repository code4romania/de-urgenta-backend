using System.Threading.Tasks;
using DeUrgenta.Courses.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Courses.Api.Validators
{
    public class CreateCourseValidator : IValidateRequest<CreateCourse>
    {
        private readonly DeUrgentaContext _context;

        public CreateCourseValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(CreateCourse request)
        {
            var isExistingUser = await _context.Users.AnyAsync(u => u.Sub == request.UserSub);

            return isExistingUser;
        }
    }
}
