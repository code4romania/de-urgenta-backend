using MediatR;
using System;

namespace DeUrgenta.Certifications.Api.Commands
{
    public class CreateCertification : IRequest<Guid>
    {
        public Guid UserId { get; }
        public string Name { get; }
        public DateTime ExpirationDate { get; }

        public CreateCertification(Guid userId, string name, DateTime expirationDate)
        {
            UserId = userId;
            Name = name;
            ExpirationDate = expirationDate;
        }
    }
}