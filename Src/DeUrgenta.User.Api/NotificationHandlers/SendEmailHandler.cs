using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DeUrgenta.User.Api.Notifications;
using DeUrgenta.User.Api.Services.Emailing;
using Microsoft.Extensions.Logging;

namespace DeUrgenta.User.Api.NotificationHandlers
{
    public class SendEmailHandler : INotificationHandler<SendEmail>
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<SendEmailHandler> _logger;

        public SendEmailHandler(IEmailSender emailSender, ILogger<SendEmailHandler> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task Handle(SendEmail notification, CancellationToken cancellationToken)
        {
            try
            {
                var email = new EmailRequestModel
                {
                    DestinationAddress = notification.DestinationAddress,
                    PlaceholderContent = notification.PlaceholderContent,
                    TemplateType = notification.TemplateType,
                    SenderName = notification.SenderName,
                    SenderEmail = notification.SenderEmail,
                    Subject = notification.Subject
                };
                await _emailSender.SendAsync(email, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Error sending email");
            }

        }
    }
}