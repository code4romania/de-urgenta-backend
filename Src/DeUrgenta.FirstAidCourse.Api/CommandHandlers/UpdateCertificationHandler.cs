using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.FirstAidCourse.Api.Commands;
using DeUrgenta.FirstAidCourse.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.FirstAidCourse.Api.CommandHandlers
{
    public class UpdateFirstAidCourseHandler : IRequestHandler<UpdateFirstAidCourse, Result<FirstAidCourseModel>>
    {
        private readonly IValidateRequest<UpdateFirstAidCourse> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateFirstAidCourseHandler(IValidateRequest<UpdateFirstAidCourse> validator, DeUrgentaContext context)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<FirstAidCourseModel>> Handle(UpdateFirstAidCourse request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<FirstAidCourseModel>("Validation failed");
            }

            var firstAidCourse = await _context.FirstAidCourses.FirstAsync(c => c.Id == request.FirstAidCourseId, cancellationToken);
            firstAidCourse.Name = request.FirstAidCourse.Name;
            firstAidCourse.IssuingAuthority = request.FirstAidCourse.IssuingAuthority;
            firstAidCourse.ExpirationDate = request.FirstAidCourse.ExpirationDate;

            await _context.SaveChangesAsync(cancellationToken);

            return new FirstAidCourseModel
            {
                Id = firstAidCourse.Id,
                Name = firstAidCourse.Name,
                ExpirationDate = firstAidCourse.ExpirationDate,
                IssuingAuthority = firstAidCourse.IssuingAuthority
            };
        }
    }
}
