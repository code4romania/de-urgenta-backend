using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;

namespace DeUrgenta.Certifications.Api.QueryHandlers
{
    public class GetCertificationPhotoHandler: IRequestHandler<GetCertificationPhoto, Result<CertificationPhotoModel>>
    {
        private readonly IValidateRequest<GetCertificationPhoto> _validator;
        private readonly DeUrgentaContext _context;

        public GetCertificationPhotoHandler(IValidateRequest<GetCertificationPhoto> validator, DeUrgentaContext context)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<CertificationPhotoModel>> Handle(GetCertificationPhoto request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<CertificationPhotoModel>("Validation failed");
            }

            var certification = await _context.Certifications.FindAsync(request.CertificationId);

            var certificationPhoto = new CertificationPhotoModel
            {
                Title = certification.PhotoTitle, 
                Photo = certification.Photo
            };

            return certificationPhoto;
        }
    }
}
