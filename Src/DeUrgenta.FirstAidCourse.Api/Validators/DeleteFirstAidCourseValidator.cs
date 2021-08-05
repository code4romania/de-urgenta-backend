using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeUrgenta.FirstAidCourse.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.FirstAidCourse.Api.Validators
{
    public class DeleteFirstAidCourseValidator : IValidateRequest<DeleteFirstAidCourse>
    {
        private readonly DeUrgentaContext _context;

        public DeleteFirstAidCourseValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(DeleteFirstAidCourse request)
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
