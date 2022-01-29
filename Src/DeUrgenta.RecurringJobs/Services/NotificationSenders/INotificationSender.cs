using System;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public interface INotificationSender
    {
        Task<bool> SendNotificationAsync(Guid notificationId);
    }
}