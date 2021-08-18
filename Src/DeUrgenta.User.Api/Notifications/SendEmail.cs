using MediatR;
using System.Collections.Generic;
using DeUrgenta.User.Api.Services.Emailing;

namespace DeUrgenta.User.Api.Notifications
{
    public class SendEmail : INotification
    {
        public string DestinationAddress { get; }
        public string SenderName { get; }
        public string SenderEmail { get; }
        public string Subject { get; }

        public Dictionary<string, string> PlaceholderContent { get; }
        public EmailTemplate TemplateType { get; }
        public EmailAttachment Attachment { get; }

        public SendEmail(string destinationAddress,
            string senderName,
            string senderEmail,
            string subject,
            EmailTemplate templateType)
        {
            DestinationAddress = destinationAddress;
            SenderName = senderName;
            SenderEmail = senderEmail;
            Subject = subject;

            TemplateType = templateType;
            PlaceholderContent = new Dictionary<string, string>();
        }

        public SendEmail(string destinationAddress,
            string senderName,
            string senderEmail,
            string subject,
            EmailTemplate templateType,
            EmailAttachment attachment) : this(destinationAddress, senderName, senderEmail, subject, templateType)
        {
            Attachment = attachment;
        }
    }
}
