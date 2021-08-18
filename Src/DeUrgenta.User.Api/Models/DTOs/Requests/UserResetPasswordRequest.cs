namespace DeUrgenta.User.Api.Models.DTOs.Requests
{
    public class UserResetPasswordRequest
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
    }
}