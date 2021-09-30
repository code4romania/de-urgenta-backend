using System;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public class NoOpNotificationSender : INotificationSender
    {
        public void SendNotification(Guid userId)
        {
            Console.WriteLine($"Sent notification to user: {userId}");
        }
    }
}