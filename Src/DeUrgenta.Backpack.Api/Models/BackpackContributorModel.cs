using System;

namespace DeUrgenta.Backpack.Api.Models
{
    public sealed record BackpackContributorModel
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public Guid UserId{ get; init; }
    }
}