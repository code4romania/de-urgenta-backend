using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
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

        public UpdateCertificationHandler(IValidateRequest<UpdateCertification> validator, DeUrgentaContext context)
        {
            _context = context;
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
            certification.PhotoTitle = request.Certification.Photo.FileName;
            certification.Photo = await GetBytesAsync(request.Certification.Photo);

            await _context.SaveChangesAsync(cancellationToken);

            return new CertificationModel
            {
                Id = certification.Id,
                Name = certification.Name,
                ExpirationDate = certification.ExpirationDate,
                IssuingAuthority = certification.IssuingAuthority
            };
        }

        private async Task<byte[]> GetBytesAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
