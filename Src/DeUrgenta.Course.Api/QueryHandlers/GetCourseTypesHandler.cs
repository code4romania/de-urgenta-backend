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
    public class GetCourseTypesHandler : IRequestHandler<GetCourseTypes, Result<IImmutableList<CourseTypeModel>>>
    {
        private readonly IValidateRequest<GetCourseTypes> _validator;
        private readonly DeUrgentaContext _context;

        public GetCourseTypesHandler(IValidateRequest<GetCourseTypes> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<CourseTypeModel>>> Handle(GetCourseTypes request,
            CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<CourseTypeModel>>("Validation failed");
            }

            var courseTypes = await _context.CourseTypes
            .Select(x => new CourseTypeModel
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync(cancellationToken);

            return courseTypes.ToImmutableList();
        }
    }
}