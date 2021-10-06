using System.Collections.Generic;

namespace DeUrgenta.Emailing.Service.Models
{
    public class EmailRequestModel
    {
        public string DestinationAddress { get; set; }
        public Dictionary<string, string> PlaceholderContent { get; set; }
        public EmailTemplate TemplateType { get; set; }
        public EmailAttachment Attachment { get; set; }
        public string Subject { get; set; }
    }
}
