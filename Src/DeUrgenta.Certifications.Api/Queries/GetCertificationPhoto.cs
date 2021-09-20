using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Models;
using MediatR;

namespace DeUrgenta.Certifications.Api.Queries
{
    public class GetCertificationPhoto : IRequest<Result<CertificationPhotoModel>>
    {
        public string UserSub { get; }
        public Guid CertificationId { get; set; }

        public GetCertificationPhoto(string userSub, Guid certificationId)
        {
            CertificationId = certificationId;
            UserSub = userSub;
        }
    }
}
