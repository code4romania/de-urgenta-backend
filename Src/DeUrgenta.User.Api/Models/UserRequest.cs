using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.User.Api.Models
{
    public sealed record UserRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string LastName { get; set; }
    }
}