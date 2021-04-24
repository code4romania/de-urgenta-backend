using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.User.Api.Models.DTOs.Requests
{
    public class UserConfirmationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
