using System;

namespace DeUrgenta.User.Api.Models
{
    public sealed record UserSafeLocationModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
    }
}