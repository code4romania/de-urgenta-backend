using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Certifications.Api.Storage;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Certifications.Api.QueryHandlers
{
    public class GetCertificationPhotoHandler : IRequestHandler<GetCertificationPhoto, Result<CertificationPhotoModel>>
    {
        private readonly IValidateRequest<GetCertificationPhoto> _validator;
        private readonly IBlobStorage _storage;

        public GetCertificationPhotoHandler(IValidateRequest<GetCertificationPhoto> validator, IBlobStorage storage)
        {
            _storage = storage;
            _validator = validator;
        }

        public async Task<Result<CertificationPhotoModel>> Handle(GetCertificationPhoto request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<CertificationPhotoModel>("Validation failed");
            }

            var photoUrl = _storage.GetAttachment(request.CertificationId, request.UserSub);

            var certificationPhoto = new CertificationPhotoModel
            {
                PhotoUrl = photoUrl
            };

            return certificationPhoto;
        }
    }
}
