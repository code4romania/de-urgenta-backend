using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Courses.Api.Commands;
using DeUrgenta.Courses.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Courses.Api.CommandHandlers
{
    public class UpdateCourseHandler : IRequestHandler<UpdateCourse, Result<CourseModel>>
    {
        private readonly IValidateRequest<UpdateCourse> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateCourseHandler(IValidateRequest<UpdateCourse> validator, DeUrgentaContext context)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<CourseModel>> Handle(UpdateCourse request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<CourseModel>("Validation failed");
            }

            var firstAidCourse = await _context.FirstAidCourses.FirstAsync(c => c.Id == request.FirstAidCourseId, cancellationToken);
            firstAidCourse.Name = request.FirstAidCourse.Name;
            firstAidCourse.IssuingAuthority = request.FirstAidCourse.IssuingAuthority;
            firstAidCourse.ExpirationDate = request.FirstAidCourse.ExpirationDate;

            await _context.SaveChangesAsync(cancellationToken);

            return new CourseModel
            {
                Id = firstAidCourse.Id,
                Name = firstAidCourse.Name,
                ExpirationDate = firstAidCourse.ExpirationDate,
                IssuingAuthority = firstAidCourse.IssuingAuthority
            };
        }
    }
}
