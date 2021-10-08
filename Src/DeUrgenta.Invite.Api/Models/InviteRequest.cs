using System;

namespace DeUrgenta.Invite.Api.Models
{
    public class InviteRequest
    {
        public Guid DestinationId { get; set; }
        public InviteType Type { get; set; }
    }
}