
namespace DeUrgenta.User.Api.Models
{
    public sealed record UserSafeLocationRequest
    {
        public string Name { get; init; }

        public decimal Latitude { get; init; }

        public decimal Longitude { get; init; }
    }
}