using CSharpFunctionalExtensions;
using DeUrgenta.Courses.Api.Models;
using DeUrgenta.Courses.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.Courses.Api.QueryHandlers
{
    public class GetCoursesHandler : IRequestHandler<GetCourses, Result<IImmutableList<CourseModel>>>
    {
        private readonly IValidateRequest<GetCourses> _validator;
        private readonly DeUrgentaContext _context;

        public GetCoursesHandler(IValidateRequest<GetCourses> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<CourseModel>>> Handle(GetCourses request,
            CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<CourseModel>>("Validation failed");
            }

            var firstAidCourse = await _context.FirstAidCourses
            .Where(x => x.User.Sub == request.UserSub)
            .Select(x => new CourseModel
            {
                Id = x.Id,
                Name = x.Name,
                IssuingAuthority = x.IssuingAuthority,
                ExpirationDate = x.ExpirationDate
            })
            .ToListAsync(cancellationToken);

            return firstAidCourse.ToImmutableList();
        }
    }
}