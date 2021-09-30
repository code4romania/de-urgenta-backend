using System;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public interface INotificationSender
    {
        void SendNotification(Guid userId);
    }
}