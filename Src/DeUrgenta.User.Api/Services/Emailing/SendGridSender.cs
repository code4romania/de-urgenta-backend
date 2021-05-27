﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DeUrgenta.User.Api.Services.Emailing
{
    public class SendGridSender : BaseEmailSender
    {
        private readonly SendGridOptions _options;
        private readonly ILogger<SendGridSender> _logger;

        public SendGridSender(IEmailBuilderService emailBuilder, SendGridOptions options, ILogger<SendGridSender> logger) : base(emailBuilder)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
        }

        public override async Task SendAsync(Email email, CancellationToken cancellationToken = default)
        {
            // note that SendGridClient doesn't implement IDisposable,
            // so if we start using this in production mode, we should refactor this
            // if there's a single api key, then we should be fine with an instance property
            // otherwise, we should be looking at creating a pool of SendGridClients
            // a nice read on this: https://github.com/sendgrid/sendgrid-csharp/issues/658
            var client = new SendGridClient(_options.ApiKey);
            var message = new SendGridMessage
            {
                From = new EmailAddress(email.FromEmail, email.FromName),
                Subject = email.Subject,
                PlainTextContent = email.Content,
                HtmlContent = email.Content
            };
            if (email.Attachment != null)
            {
                message.Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Filename = email.Attachment.FileName,
                        Content = Convert.ToBase64String(email.Attachment.Content)
                    }
                };
            }

            message.AddTo(new EmailAddress(email.To));
            message.SetClickTracking(_options.ClickTracking, _options.ClickTracking);

            _logger.LogInformation("Sending email using Sendgrid");

            var sendgridResponse = await client.SendEmailAsync(message, cancellationToken);
            var statusCode = (int)sendgridResponse.StatusCode;
            if (statusCode > 226 || statusCode < 200)
            {
                // not ok response received
                var responseMessage = await sendgridResponse.Body.ReadAsStringAsync();
                _logger.LogWarning("Received not ok(200) status code. Status code received {statusCode}. Response message {responseMessage}", statusCode, responseMessage);
            }
        }
    }
}
