namespace DeUrgenta.User.Api.Models
{
    public sealed record UserModel
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}