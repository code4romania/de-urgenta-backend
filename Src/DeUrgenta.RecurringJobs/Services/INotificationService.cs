using System;
using System.Threading.Tasks;

namespace DeUrgenta.RecurringJobs.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Guid userId);
    }
}
