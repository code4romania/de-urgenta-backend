using System;

namespace DeUrgenta.User.Api.Models
{
    public sealed record GroupInviteModel
    {
        public Guid InviteId { get; init; }
        public Guid GroupId { get; init; }
        public string GroupName { get; init; }
        public Guid SenderId { get; init; }
        public string SenderFirstName { get; init; }
        public string SenderLastName { get; init; }
    }
}