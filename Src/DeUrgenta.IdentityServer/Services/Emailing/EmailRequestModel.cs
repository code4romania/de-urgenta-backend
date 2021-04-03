﻿using System.Collections.Generic;

namespace DeUrgenta.IdentityServer.Services.Emailing
{
    public class EmailRequestModel
    {
        public string Address { get; set; }
        public Dictionary<string, string> PlaceholderContent { get; set; }
        public EmailTemplate TemplateType { get; set; }
        public EmailAttachment Attachment { get; set; }
        public string Subject { get; set; }
        public string SenderName { get; set; }
    }
}
