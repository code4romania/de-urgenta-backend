using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Models;
using MediatR;
using System;
using DeUrgenta.Common.Validation;
using Microsoft.AspNetCore.Http;

namespace DeUrgenta.Certifications.Api.Commands
{
    public class CreateCertification : IRequest<Result<CertificationModel, ValidationResult>>
    {
        public string UserSub { get; }
        public string Name { get; }
        public DateTime ExpirationDate { get; }
        public string IssuingAuthority { get; }
        public IFormFile Photo { get; init; }

        public CreateCertification(string userSub, CertificationRequest certificationRequest)
        {
            UserSub = userSub;
            Name = certificationRequest.Name;
            ExpirationDate = certificationRequest.ExpirationDate;
            IssuingAuthority = certificationRequest.IssuingAuthority;
            Photo = certificationRequest.Photo;
        }
    }
}