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
    public class UpdateCourseHandler : IRequestHandler<UpdateCourse, Result<CourseTypeModel>>
    {
        private readonly IValidateRequest<UpdateCourse> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateCourseHandler(IValidateRequest<UpdateCourse> validator, DeUrgentaContext context)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<CourseTypeModel>> Handle(UpdateCourse request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<CourseTypeModel>("Validation failed");
            }

            var course = await _context.Courses.FirstAsync(c => c.Id == request.CourseId, cancellationToken);
            course.Name = request.Course.Name;
            course.IssuingAuthority = request.Course.IssuingAuthority;
            course.ExpirationDate = request.Course.ExpirationDate;

            await _context.SaveChangesAsync(cancellationToken);

            return new CourseTypeModel
            {
                Id = course.Id,
                Name = course.Name,
            };
        }
    }
}
