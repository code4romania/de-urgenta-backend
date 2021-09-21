using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Storage;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.CommandHandlers
{
    public class UpdateCertificationHandler : IRequestHandler<UpdateCertification, Result<CertificationModel>>
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

        public async Task<Result<CertificationModel>> Handle(UpdateCertification request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<CertificationModel>("Validation failed");
            }

            var certification = await _context.Certifications.FirstAsync(c => c.Id == request.CertificationId, cancellationToken);
            certification.Name = request.Certification.Name;
            certification.IssuingAuthority = request.Certification.IssuingAuthority;
            certification.ExpirationDate = request.Certification.ExpirationDate;
           
            await _context.SaveChangesAsync(cancellationToken);

            var photoStream = await GetStreamAsync(request.Certification.Photo);
            var fileUrl = await _storage.SaveAttachmentAsync(certification.Id, request.UserSub, photoStream);

            return new CertificationModel
            {
                Id = certification.Id,
                Name = certification.Name,
                ExpirationDate = certification.ExpirationDate,
                IssuingAuthority = certification.IssuingAuthority,
                PhotoUrl = fileUrl
            };
        }

        private async Task<MemoryStream> GetStreamAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream;
        }
    }
}
