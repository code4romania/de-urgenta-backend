using System;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public class NoOpNotificationSender : INotificationSender
    {
        public Task SendNotificationAsync(Guid notificationId)
        {
            Console.WriteLine($"Sent notification Id '{notificationId}' to user.");

            return Task.CompletedTask;
        }
    }
}