using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.User.Api.Models.DTOs.Requests
{
    public class UserChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}