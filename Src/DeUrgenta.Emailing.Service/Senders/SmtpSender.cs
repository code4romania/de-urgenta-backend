using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Emailing.Service.Builders;
using DeUrgenta.Emailing.Service.Config;
using DeUrgenta.Emailing.Service.Constants;
using DeUrgenta.Emailing.Service.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Polly.Registry;
using Polly.Retry;

namespace DeUrgenta.Emailing.Service.Senders
{
    public class SmtpSender : BaseEmailSender
    {
        private readonly SmtpOptions _options;
        private readonly ILogger<SmtpSender> _logger;
        private readonly IReadOnlyPolicyRegistry<string> _registry;

        public SmtpSender(IEmailBuilderService emailBuilder, IOptions<SmtpOptions> options, ILogger<SmtpSender> logger, IReadOnlyPolicyRegistry<string> registry) : base(emailBuilder)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _registry = registry;
        }

        public override async Task<bool> SendAsync(Email email, CancellationToken cancellationToken = default)
        {
            var message = BuildMimeMessage(email);

            var retryPolicy = (AsyncRetryPolicy<bool>)_registry[PolicyNameConstants.SmtpPolicyName];

            try
            {
                var result = await retryPolicy
                    .ExecuteAsync(() => SendEmailAsync(message, cancellationToken));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email via Smtp: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SendEmailAsync(MimeMessage message, CancellationToken cancellationToken)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.Auto, cancellationToken);
                await client.AuthenticateAsync(_options.User, _options.Password, cancellationToken);
                
                _logger.LogInformation("Sending email using SmtpSender");
                
                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);
            }
            return true;
        }

        private static MimeMessage BuildMimeMessage(Email email)
        {
            var body = new TextPart(TextFormat.Html) { Text = email.Content };
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(email.FromName, email.FromEmail));
            message.Sender = new MailboxAddress(email.FromName, email.FromEmail);
            message.Subject = email.Subject;
            message.Body = body;
            message.To.Add(MailboxAddress.Parse(email.To));

            if (email.Attachment != null)
            {
                var attachment = new MimePart("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    Content = new MimeContent(new MemoryStream(email.Attachment.Content)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = email.Attachment.FileName
                };
                var multipart = new Multipart("mixed") { body, attachment };

                message.Body = multipart;
            }
            else
            {
                message.Body = body;
            }

            return message;
        }
    }
}
