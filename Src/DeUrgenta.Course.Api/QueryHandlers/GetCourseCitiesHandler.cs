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
    public class GetCourseCitiesHandler : IRequestHandler<GetCourseCities, Result<IImmutableList<CourseCityModel>>>
    {
        private readonly IValidateRequest<GetCourseCities> _validator;
        private readonly DeUrgentaContext _context;

        public GetCourseCitiesHandler(IValidateRequest<GetCourseCities> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<CourseCityModel>>> Handle(GetCourseCities request,
            CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<CourseCityModel>>("Validation failed");
            }

            var courseCities = await _context.CourseCities
            .Select(x => new CourseCityModel
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToListAsync(cancellationToken);

            return courseCities.ToImmutableList();
        }
    }
}