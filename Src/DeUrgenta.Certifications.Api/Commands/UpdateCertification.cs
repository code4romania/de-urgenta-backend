using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Certifications.Api.Commands
{
    public class UpdateCertification : IRequest<Result<CertificationModel, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid CertificationId { get; }
        public CertificationRequest Certification { get; }
        public UpdateCertification(string sub, Guid certificationId, CertificationRequest certification)
        {
            UserSub = sub;
            CertificationId = certificationId;
            Certification = certification;
        }
    }
}
