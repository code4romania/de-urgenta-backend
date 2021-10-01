using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.RecurringJobs.Domain.Entities
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }
        public NotificationType Type { get; set; }
        public Guid UserId { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime ScheduledDate { get; set; }

        public virtual CertificationDetails CertificationDetails{ get; set; }
    }
}
