using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Courses.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Courses.Api.CommandHandlers
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourse, Result>
    {
        private readonly IValidateRequest<DeleteCourse> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteCourseHandler(IValidateRequest<DeleteCourse> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result> Handle(DeleteCourse request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var course = await _context.Courses.FirstAsync(c => c.Id == request.courseId, cancellationToken);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

    }
}
