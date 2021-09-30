using System;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public class NoOpNotificationSender : INotificationSender
    {
        public Task SendNotificationAsync(Guid userId)
        {
            Console.WriteLine($"Sent notification to user: {userId}");

            return Task.CompletedTask;
        }
    }
}