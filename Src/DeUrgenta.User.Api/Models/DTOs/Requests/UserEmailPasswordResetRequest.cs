using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.User.Api.Models.DTOs.Requests
{
    public class UserEmailPasswordResetRequest
    {
        public string Email { get; set; }
    }
}