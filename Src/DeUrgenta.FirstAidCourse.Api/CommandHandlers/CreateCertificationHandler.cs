using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.FirstAidCourse.Api.Commands;
using DeUrgenta.FirstAidCourse.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.FirstAidCourse.Api.CommandHandlers
{
    public class CreateCertificationHandler : IRequestHandler<CreateFirstAidCourse, Result<FirstAidCourseModel>>
    {
        private readonly IValidateRequest<CreateFirstAidCourse> _validator;
        private readonly DeUrgentaContext _context;

        public CreateCertificationHandler(IValidateRequest<CreateFirstAidCourse> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<FirstAidCourseModel>> Handle(CreateFirstAidCourse request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<FirstAidCourseModel>("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var certification = new FirstAidCourse
            {
                Name = request.Name,
                ExpirationDate = request.ExpirationDate,
                User = user,
                IssuingAuthority = request.IssuingAuthority
            };

            await _context.FirstAidCourses.AddAsync(certification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new FirstAidCourseModel
            {
                Id = certification.Id,
                Name = certification.Name,
                ExpirationDate = certification.ExpirationDate,
                IssuingAuthority = certification.IssuingAuthority
            };
        }
    }
}
