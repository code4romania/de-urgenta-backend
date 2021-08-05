using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.FirstAidCourse.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.FirstAidCourse.Api.CommandHandlers
{
    public class DeleteFirstAidCourseHandler : IRequestHandler<DeleteFirstAidCourse, Result>
    {
        private readonly IValidateRequest<DeleteFirstAidCourse> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteFirstAidCourseHandler(IValidateRequest<DeleteFirstAidCourse> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result> Handle(DeleteFirstAidCourse request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var firstAidCourse = await _context.FirstAidCourses.FirstAsync(c => c.Id == request.FirstAidCourseId, cancellationToken);
            _context.FirstAidCourses.Remove(firstAidCourse);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

    }
}
