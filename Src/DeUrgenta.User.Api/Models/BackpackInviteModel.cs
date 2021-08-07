using System;

namespace DeUrgenta.User.Api.Models
{
    public sealed record BackpackInviteModel
    {
        public Guid InviteId { get; init; }
        public Guid BackpackId { get; init; }
        public string BackpackName { get; init; }
        public Guid SenderId { get; init; }
        public string SenderFirstName { get; init; }
        public string SenderLastName { get; init; }
    }
}