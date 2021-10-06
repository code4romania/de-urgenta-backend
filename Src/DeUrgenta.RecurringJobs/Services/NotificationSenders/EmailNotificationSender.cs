using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Emailing.Service.Models;
using DeUrgenta.Emailing.Service.Senders;
using DeUrgenta.RecurringJobs.Domain;
using DeUrgenta.RecurringJobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public class EmailNotificationSender : INotificationSender
    {
        private readonly JobsContext _jobsContext;
        private readonly DeUrgentaContext _context;
        private readonly IEmailSender _emailSender;

        public EmailNotificationSender(JobsContext jobsContext, DeUrgentaContext context, IEmailSender emailSender)
        {
            _jobsContext = jobsContext;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task SendNotificationAsync(Guid notificationId)
        {
            var notification = await _jobsContext.Notifications
                .Include(n => n.CertificationDetails)
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
            {
                return;
            }

            if (notification.Type == NotificationType.Certification)
            {
                var email = await CreateExpiredCertificationEmail(notification);

                await _emailSender.SendAsync(email);
            }
        }

        //todo extract this in a builder when we add the next notification type
        private async Task<EmailRequestModel> CreateExpiredCertificationEmail(Notification notification)
        {
            var user = await _context.Users.FindAsync(notification.UserId);
            var certification = await _context.Certifications.FindAsync(notification.CertificationDetails.CertificationId);

            var email = new EmailRequestModel
            {
                DestinationAddress = user.Email,
                Subject = "",
                TemplateType = EmailTemplate.ExpiredCertification,
                PlaceholderContent = new Dictionary<string, string>
                {
                    { "certificationName", certification.Name },
                    { "certificationExpirationDate", certification.ExpirationDate.ToString("d-MM-yyyy") }
                }
            };
            return email;
        }
    }
}