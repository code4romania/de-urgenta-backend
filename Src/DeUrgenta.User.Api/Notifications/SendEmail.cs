using MediatR;
using System.Collections.Generic;
using DeUrgenta.Emailing.Service.Models;

namespace DeUrgenta.User.Api.Notifications
{
    public class SendEmail : INotification
    {
        public string DestinationAddress { get; }
        public string Subject { get; }

        public Dictionary<string, string> PlaceholderContent { get; }
        public EmailTemplate TemplateType { get; }
        public EmailAttachment Attachment { get; }

        public SendEmail(string destinationAddress,
            string subject,
            EmailTemplate templateType)
        {
            DestinationAddress = destinationAddress;
            Subject = subject;

            TemplateType = templateType;
            PlaceholderContent = new Dictionary<string, string>();
        }

        public SendEmail(string destinationAddress,
            string subject,
            EmailTemplate templateType,
            EmailAttachment attachment) : this(destinationAddress, subject, templateType)
        {
            Attachment = attachment;
        }
    }
}
