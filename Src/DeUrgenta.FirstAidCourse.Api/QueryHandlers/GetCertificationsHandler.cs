using CSharpFunctionalExtensions;
using DeUrgenta.FirstAidCourse.Api.Models;
using DeUrgenta.FirstAidCourse.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.FirstAidCourse.Api.QueryHandlers
{
    public class GetFirstAidCoursesHandler : IRequestHandler<GetFirstAidCourses, Result<IImmutableList<FirstAidCourseModel>>>
    {
        private readonly IValidateRequest<GetFirstAidCourses> _validator;
        private readonly DeUrgentaContext _context;

        public GetFirstAidCoursesHandler(IValidateRequest<GetFirstAidCourses> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<FirstAidCourseModel>>> Handle(GetFirstAidCourses request,
            CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<FirstAidCourseModel>>("Validation failed");
            }

            var firstAidCourse = await _context.FirstAidCourses
            .Where(x => x.User.Sub == request.UserSub)
            .Select(x => new FirstAidCourseModel
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