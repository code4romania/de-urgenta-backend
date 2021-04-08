using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.User.Api.Models
{
    public sealed record UserSafeLocationRequest
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