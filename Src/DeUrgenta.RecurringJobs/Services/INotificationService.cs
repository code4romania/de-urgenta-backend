using System;
using System.Threading.Tasks;
using DeUrgenta.Domain.RecurringJobs.Entities;

namespace DeUrgenta.RecurringJobs.Services
{
    public interface INotificationService
    {
        Task<NotificationStatus> SendNotificationAsync(Guid notificationId);
    }
}
