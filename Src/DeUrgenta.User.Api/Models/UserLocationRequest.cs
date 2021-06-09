using System.ComponentModel.DataAnnotations;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.User.Api.Models
{
    public sealed record UserLocationRequest
    {
        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string Address { get; init; }

        [Required]
        public decimal Latitude { get; init; }

        [Required]
        public decimal Longitude { get; init; }

        [Required]
        public UserAddressType Type { get; set; }
    }
}