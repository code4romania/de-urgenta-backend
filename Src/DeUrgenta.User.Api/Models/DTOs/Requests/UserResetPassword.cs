using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.User.Api.Models.DTOs.Requests
{
    public class UserResetPassword
    {
        [Required]
        public string UserId {get;set;}

        [Required]
        public string ResetToken {get;set;}

        [Required]
        public string NewPassword {get;set;}

        [Required]
        [Compare("NewPassword",ErrorMessage ="The passwords do not match")]
        public string NewPasswordConfirm {get;set;}
    }
}