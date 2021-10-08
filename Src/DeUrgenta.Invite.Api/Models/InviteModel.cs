using System;

namespace DeUrgenta.Invite.Api.Models
{
    public class InviteModel
    {
        public Guid Id { get; set; }
        public Guid DestinationId { get; set; }
        public InviteType Type { get; set; }
        public DateTime SentOn { get; set; }
    }
}