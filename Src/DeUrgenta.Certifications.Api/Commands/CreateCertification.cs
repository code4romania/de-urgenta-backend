using MediatR;
using System;

namespace DeUrgenta.Certifications.Api.Commands
{
    public class CreateCertification : IRequest<int>
    {
        public int UserId { get; }
        public string Name { get; }
        public DateTime ExpirationDate { get; }

        public CreateCertification(int userId, string name, DateTime expirationDate)
        {
            UserId = userId;
            Name = name;
            ExpirationDate = expirationDate;
        }
    }
}