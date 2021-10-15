using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.Domain.RecurringJobs.Entities
{
    public class ItemDetails
    {
        public Guid ItemId { get; set; }

        [Key]
        public virtual Guid NotificationId { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
