using System;

namespace DeUrgenta.Invite.Api.Models
{
    public record AcceptInviteRequest
    {
        public Guid InviteId { get; set; }
    }
}