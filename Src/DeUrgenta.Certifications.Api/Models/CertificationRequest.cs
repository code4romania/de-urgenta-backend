using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Certifications.Api.Models
{
    public record CertificationRequest
    {
        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Name { get; init; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string IssuingAuthority { get; init; }

        [Required]
        public DateTime ExpirationDate { get; init; }
    }
}
