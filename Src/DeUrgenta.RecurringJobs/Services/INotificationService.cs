using System;

namespace DeUrgenta.RecurringJobs.Services
{
    public interface INotificationService
    {
        void SendNotification(Guid userId);
    }
}
