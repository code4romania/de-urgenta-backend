using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Group.Api.Models
{
    public sealed record SafeLocationRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; init; }

        [Required]
        public decimal Latitude { get; init; }

        [Required]
        public decimal Longitude { get; init; }
    }
}