
namespace DeUrgenta.Group.Api.Models
{
    public sealed record SafeLocationRequest
    {
        public string Name { get; init; }

        public decimal Latitude { get; init; }

        public decimal Longitude { get; init; }
    }
}