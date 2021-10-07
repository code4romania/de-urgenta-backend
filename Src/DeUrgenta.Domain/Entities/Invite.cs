using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class Invite
    {
        [Key]
        public Guid Id { get; set; }

        public InviteType Type { get; set; }

        public Guid DestinationId { get; set; }

        public InviteStatus InviteStatus { get; set; }

        public DateTime SentOn { get; set; }
    }
}
