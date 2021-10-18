using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Emailing.Service.Builders;
using DeUrgenta.Emailing.Service.Config;
using DeUrgenta.Emailing.Service.Constants;
using DeUrgenta.Emailing.Service.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly.Registry;
using Polly.Retry;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DeUrgenta.Emailing.Service.Senders
{
    public class SendGridSender : BaseEmailSender
    {
        private readonly SendGridOptions _options;
        private readonly ILogger<SendGridSender> _logger;
        private readonly IReadOnlyPolicyRegistry<string> _registry;

        public SendGridSender(IEmailBuilderService emailBuilder, IOptions<SendGridOptions> options, ILogger<SendGridSender> logger, IReadOnlyPolicyRegistry<string> registry) : base(emailBuilder)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
            _registry = registry;
        }

        public override async Task<bool> SendAsync(Email email, CancellationToken cancellationToken = default)
        {
            var message = BuildSendGridMessage(email);

            var retryPolicy = (AsyncRetryPolicy<HttpStatusCode>)_registry[PolicyNameConstants.SendGridPolicyName];
            var statusCode = await retryPolicy.ExecuteAsync(() => SendEmailAsync(message, cancellationToken));

            return statusCode >= HttpStatusCode.OK && statusCode <= (HttpStatusCode)299;
        }

        private async Task<HttpStatusCode> SendEmailAsync(SendGridMessage message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending email using Sendgrid");

            // note that SendGridClient doesn't implement IDisposable,
            // so if we start using this in production mode, we should refactor this
            // if there's a single api key, then we should be fine with an instance property
            // otherwise, we should be looking at creating a pool of SendGridClients
            // a nice read on this: https://github.com/sendgrid/sendgrid-csharp/issues/658
            var client = new SendGridClient(_options.ApiKey);
            var sendGridResponse = await client.SendEmailAsync(message, cancellationToken);
            if (!sendGridResponse.IsSuccessStatusCode)
            {
                var responseMessage = await sendGridResponse.Body.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning(
                    "SendGrid send email failed. Status code received {statusCode}. Response message {responseMessage}",
                    sendGridResponse.StatusCode, responseMessage);
            }

            return sendGridResponse.StatusCode;
        }

        private SendGridMessage BuildSendGridMessage(Email email)
        {
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
                    new()
                    {
                        Filename = email.Attachment.FileName,
                        Content = Convert.ToBase64String(email.Attachment.Content)
                    }
                };
            }

            message.AddTo(new EmailAddress(email.To));
            message.SetClickTracking(_options.ClickTracking, _options.ClickTracking);
            return message;
        }
    }
}
