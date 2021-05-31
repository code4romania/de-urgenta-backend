using CSharpFunctionalExtensions;
using DeUrgenta.Certifications.Api.Models;
using MediatR;
using System;

namespace DeUrgenta.Certifications.Api.Commands
{
    public class CreateCertification : IRequest<Result<CertificationModel>>
    {
        public string UserSub{ get; }
        public string Name { get; }
        public DateTime ExpirationDate { get; }

        public CreateCertification(string userSub, string name, DateTime expirationDate)
        {
            UserSub = userSub;
            Name = name;
            ExpirationDate = expirationDate;
        }
    }
}