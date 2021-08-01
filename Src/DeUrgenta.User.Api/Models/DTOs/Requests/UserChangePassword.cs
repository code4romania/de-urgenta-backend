using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.User.Api.Models.DTOs.Requests
{
    public class UserChangePassword
    {
        [Required]
        public string OldPassword {get;set;}

        [Required]
        public string NewPassword {get;set;}

        [Required]
        [Compare("NewPassword",ErrorMessage ="The passwords do not match")]
        public string ConfirmNewPassword {get;set;}
    }
}