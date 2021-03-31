using System;

namespace DeUrgenta.Certifications.Api.Models
{
    public record CertificationModel(int Id, string Name, DateTime ExpirationDate);
}
