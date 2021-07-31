using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.User.Api.Models.DTOs.Requests
{
    public class UserChangePassword
    {
        [Required]
        public string OldPassword {get;set;}

        [Required]
        public string NewPassword {get;set;}
    }
}