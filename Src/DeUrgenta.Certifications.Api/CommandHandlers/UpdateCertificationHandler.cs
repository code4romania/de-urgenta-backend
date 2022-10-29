using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Storage;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.CommandHandlers
{
    public class UpdateCertificationHandler : IRequestHandler<UpdateCertification, Result<CertificationModel, ValidationResult>>
    {
        private readonly IValidateRequest<UpdateCertification> _validator;
        private readonly DeUrgentaContext _context;
        private readonly IBlobStorage _storage;

        public UpdateCertificationHandler(IValidateRequest<UpdateCertification> validator, DeUrgentaContext context, IBlobStorage storage)
        {
            _context = context;
            _storage = storage;
            _validator = validator;
        }

        public async Task<Result<CertificationModel, ValidationResult>> Handle(UpdateCertification request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var certification = await _context.Certifications
                .FirstAsync(c => c.Id == request.CertificationId, ct);
            certification.Name = request.Certification.Name;
            certification.IssuingAuthority = request.Certification.IssuingAuthority;
            certification.ExpirationDate = request.Certification.ExpirationDate;

            await _context.SaveChangesAsync(ct);

            string photoUrl = null;
            if (request.Certification.Photo != null)
            {
                photoUrl = await _storage.SaveAttachmentAsync(certification.Id,
                    request.UserSub,
                    request.Certification.Photo.OpenReadStream(),
                    Path.GetExtension(request.Certification.Photo.FileName),
                    ct);
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
