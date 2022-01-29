using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public class NoOpNotificationSender : INotificationSender
    {
        private readonly ILogger<NoOpNotificationSender> _logger;

        public NoOpNotificationSender(ILogger<NoOpNotificationSender> logger)
        {
            _logger = logger;
        }

        public Task<bool> SendNotificationAsync(Guid notificationId)
        {
            _logger.LogInformation($"Sent notification Id '{notificationId}' to user.");

            return Task.FromResult(true);
        }
    }
}