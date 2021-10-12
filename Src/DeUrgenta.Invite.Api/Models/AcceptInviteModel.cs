using System;

namespace DeUrgenta.Invite.Api.Models
{
    public record AcceptInviteModel
    {
        public InviteType Type { get; set; }
        public Guid DestinationId { get; set; }
        public string DestinationName { get; set; }
    }
}
