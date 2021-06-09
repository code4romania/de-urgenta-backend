using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.User.Api.Models
{
    public sealed record UserLocationModel
    {
        public Guid Id { get; init; }
        public string Address { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
        public UserLocationType Type { get; init; }
    }
}