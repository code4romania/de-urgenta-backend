using System;

namespace DeUrgenta.User.Api.Models
{
    public sealed record UserModel
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}