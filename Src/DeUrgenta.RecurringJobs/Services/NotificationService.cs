﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task SendNotificationAsync(Guid userId)
        {
            if (_notificationSenders == null || !_notificationSenders.Any())
            {
                _logger.LogWarning("No NotificationSenders configured. please revise the application settings");
                return;
            }

            foreach (var notificationSender in _notificationSenders)
            {
                await notificationSender.SendNotificationAsync(userId);
            }
        }
    }
}