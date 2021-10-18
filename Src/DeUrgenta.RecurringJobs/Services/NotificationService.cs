using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Domain.RecurringJobs.Entities;
using DeUrgenta.RecurringJobs.Services.NotificationSenders;
using Microsoft.Extensions.Logging;

namespace DeUrgenta.RecurringJobs.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEnumerable<INotificationSender> _notificationSenders;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IEnumerable<INotificationSender> notificationSenders, ILogger<NotificationService> logger)
        {
            _notificationSenders = notificationSenders;
            _logger = logger;
        }

        public async Task<NotificationStatus> SendNotificationAsync(Guid notificationId)
        {
            if (_notificationSenders == null || !_notificationSenders.Any())
            {
                _logger.LogWarning("No NotificationSenders configured. please revise the application settings");
                return NotificationStatus.NotSent;
            }

            var sendingTasks = new List<Task<bool>>();
            foreach (var notificationSender in _notificationSenders)
            {
                sendingTasks.Add(notificationSender.SendNotificationAsync(notificationId));
            }
            var sentSuccessful = await Task.WhenAll(sendingTasks);

            var notificationStatus = sentSuccessful.All(b => b)
                ? NotificationStatus.Sent
                : (sentSuccessful.Any(b => b)
                    ? NotificationStatus.PartlySent
                    : NotificationStatus.ErrorSending);
            return notificationStatus;
        }
    }
}