using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Courses.Api.Commands;
using DeUrgenta.Courses.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Courses.Api.CommandHandlers
{
    public class CreateCourseHandler : IRequestHandler<CreateCourse, Result<CourseTypeModel>>
    {
        private readonly IValidateRequest<CreateCourse> _validator;
        private readonly DeUrgentaContext _context;

        public CreateCourseHandler(IValidateRequest<CreateCourse> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<CourseTypeModel>> Handle(CreateCourse request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<CourseTypeModel>("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var certification = new Course
            {
                Name = request.Name,
                ExpirationDate = request.ExpirationDate,
                User = user,
                IssuingAuthority = request.IssuingAuthority
            };

            await _context.Courses.AddAsync(certification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CourseTypeModel
            {
                Id = certification.Id,
                Name = certification.Name,
            };
        }
    }
}
