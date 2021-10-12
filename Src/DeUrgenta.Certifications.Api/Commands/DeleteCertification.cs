using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Certifications.Api.Commands
{
    public class DeleteCertification : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid CertificationId { get; }
        public DeleteCertification(string sub, Guid certificationId)
        {
            UserSub = sub;
            CertificationId = certificationId;
        }
    }
}
