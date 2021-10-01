using System;
using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.RecurringJobs.Domain.Entities
{
    public class CertificationDetails
    {
        [Key]
        public Guid Id { get; set; }
        
        public virtual Notification Notification { get; set; }
        public virtual Guid NotificationId { get; set; }
    }
}