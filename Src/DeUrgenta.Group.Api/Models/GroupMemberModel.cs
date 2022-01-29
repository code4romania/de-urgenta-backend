using System;

namespace DeUrgenta.Group.Api.Models
{
    public sealed record GroupMemberModel
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public bool IsGroupAdmin { get; init; }
        public bool HasValidCertification { get; init; }

    }
}