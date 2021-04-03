using System;

namespace DeUrgenta.Certifications.Api.Models
{
    public sealed record CertificationModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string IssuingAuthority { get; init; }
        public DateTime ExpirationDate { get; init; }
    }
}
