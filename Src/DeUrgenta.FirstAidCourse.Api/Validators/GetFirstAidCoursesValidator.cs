using System.Threading.Tasks;
using DeUrgenta.FirstAidCourse.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.FirstAidCourse.Api.Validators
{
    public class GetFirstAidCoursesValidator : IValidateRequest<GetFirstAidCourses>
    {
        private readonly DeUrgentaContext _context;

        public GetFirstAidCoursesValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetFirstAidCourses request)
        {
            var isExistingUser = await _context.Users.AnyAsync(u => u.Sub == request.UserSub);

            return isExistingUser;
        }
    }
}
