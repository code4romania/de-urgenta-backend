using System.Threading.Tasks;
using DeUrgenta.Courses.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Courses.Api.Validators
{
    public class GetFirstAidCoursesValidator : IValidateRequest<GetCourses>
    {
        private readonly DeUrgentaContext _context;

        public GetFirstAidCoursesValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetCourses request)
        {
            var isExistingUser = await _context.Users.AnyAsync(u => u.Sub == request.UserSub);

            return isExistingUser;
        }
    }
}
