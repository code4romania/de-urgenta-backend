﻿using System;
using System.Threading.Tasks;
using DeUrgenta.Emailing.Service.Senders;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.RecurringJobs.Services.NotificationSenders.EmailBuilders;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public class EmailNotificationSender : INotificationSender
    {
        private readonly JobsContext _jobsContext;
        private readonly IEmailSender _emailSender;
        private readonly EmailRequestBuilderFactory _factory;

        public EmailNotificationSender(JobsContext jobsContext, IEmailSender emailSender, EmailRequestBuilderFactory factory)
        {
            _jobsContext = jobsContext;
            _emailSender = emailSender;
            _factory = factory;
        }

        public async Task<bool> SendNotificationAsync(Guid notificationId)
        {
            var notification = await _jobsContext.Notifications
                .Include(n => n.CertificationDetails)
                .Include(n => n.ItemDetails)
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
            {
                return false;
            }

            var emailRequestBuilder = _factory.GetBuilderInstance(notification.Type);

            var email = await emailRequestBuilder.CreateEmailRequest(notification);
            return await _emailSender.SendAsync(email);
        }
    }
}