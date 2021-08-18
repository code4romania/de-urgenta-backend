
namespace DeUrgenta.User.Api.Models
{
    public sealed record UserRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}