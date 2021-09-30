using System;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public interface INotificationSender
    {
        Task SendNotificationAsync(Guid userId);
    }
}