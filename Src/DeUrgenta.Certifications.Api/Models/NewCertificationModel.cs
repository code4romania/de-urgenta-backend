using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Certifications.Api.Models
{
    public record NewCertificationModel
    {
        /// <summary>
        /// The name of the product
        /// </summary>
        /// <example>Men's basketball shoes</example>
        [Required]
        [MaxLength(250)]
        public string Name { get; init; }

        [Required]
        public DateTime ExpirationDate { get; init; }
    }
}
