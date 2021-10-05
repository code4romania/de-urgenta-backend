using System;
using DeUrgenta.RecurringJobs.Domain.Entities;

namespace DeUrgenta.RecurringJobs.Tests.Builders
{
    public class NotificationBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private Guid _id = Guid.NewGuid();
        private DateTime _scheduledDate = DateTime.Today;
        private Guid _certificationId = Guid.NewGuid();
        private NotificationStatus _status = NotificationStatus.NotSent;

        public Notification Build() =>
            new()
            {
                Status = _status,
                Type = NotificationType.Certification,
                UserId = _userId,
                Id = _id,
                ScheduledDate = _scheduledDate,
                CertificationDetails = new CertificationDetails
                {
                    CertificationId = _certificationId
                }
            };

        public NotificationBuilder WithScheduledDate(DateTime notificationDate)
        {
            _scheduledDate = notificationDate;
            return this;
        }

        public NotificationBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public NotificationBuilder WithCertificationId(Guid certificationId)
        {
            _certificationId = certificationId;
            return this;
        }

        public NotificationBuilder WithStatus(NotificationStatus notificationStatus)
        {
            _status = notificationStatus;
            return this;
        }
    }
}
