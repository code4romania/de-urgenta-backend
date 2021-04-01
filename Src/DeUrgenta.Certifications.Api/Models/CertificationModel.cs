using System;

namespace DeUrgenta.Certifications.Api.Models
{
    public record CertificationModel(Guid Id, string Name, DateTime ExpirationDate);
}
