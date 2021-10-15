using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Storage;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.CommandHandlers
{
    public class CreateCertificationHandler : IRequestHandler<CreateCertification, Result<CertificationModel, ValidationResult>>
    {
        private readonly IValidateRequest<CreateCertification> _validator;
        private readonly DeUrgentaContext _context;
        private readonly IBlobStorage _storage;

        public CreateCertificationHandler(IValidateRequest<CreateCertification> validator, DeUrgentaContext context, IBlobStorage storage)
        {
            _validator = validator;
            _context = context;
            _storage = storage;
        }

        public async Task<Result<CertificationModel, ValidationResult>> Handle(CreateCertification request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var certification = new Certification
            {
                Name = request.Name,
                ExpirationDate = request.ExpirationDate,
                User = user,
                IssuingAuthority = request.IssuingAuthority
            };

            await _context.Certifications.AddAsync(certification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            string photoUrl = null;
            if (request.Photo != null)
            {
                photoUrl = await _storage.SaveAttachmentAsync(certification.Id,
                    user.Sub,
                    request.Photo.OpenReadStream(),
                    Path.GetExtension(request.Photo.FileName));
            }

            return new CertificationModel
            {
                Id = certification.Id,
                Name = certification.Name,
                ExpirationDate = certification.ExpirationDate,
                IssuingAuthority = certification.IssuingAuthority,
                PhotoUrl = photoUrl
            };
        }
    }
}
