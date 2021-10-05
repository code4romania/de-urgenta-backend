using System;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public class PushNotificationSender : INotificationSender
    {
        public Task SendNotificationAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }
    }
}