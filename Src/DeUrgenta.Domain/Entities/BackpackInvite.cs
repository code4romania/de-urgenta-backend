using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class BackpackInvite
    {
        [Key]
        public Guid Id { get; set; }

        public virtual Guid BackpackId { get; set; }
        public virtual Backpack Backpack { get; set; }

        public Guid InvitationSenderId { get; set; }
        public virtual User InvitationSender { get; set; }

        public Guid InvitationReceiverId { get; set; }
        public virtual User InvitationReceiver { get; set; }

    }
}