using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.Entities
{
    public class GroupInvite
    {
        [Key]
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }
        public virtual Group Group { get; set; }

        public Guid InvitationSenderId { get; set; }
        public virtual User InvitationSender { get; set; }

        public Guid InvitationReceiverId { get; set; }
        public virtual User InvitationReceiver { get; set; }

    }
}