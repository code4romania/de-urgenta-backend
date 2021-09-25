using System.IO;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DeUrgenta.User.Api.Services.Emailing
{
    public class EmailBuilderService : IEmailBuilderService
    {
        private readonly ILogger<IEmailBuilderService> _logger;
        private readonly ITemplateFileSelector _templateFileSelector;
        private readonly IMemoryCache _memoryCache;

        public EmailBuilderService(ILogger<IEmailBuilderService> logger, ITemplateFileSelector templateFileSelector, IMemoryCache memoryCache)
        {
            _logger = logger;
            _templateFileSelector = templateFileSelector;
            _memoryCache = memoryCache;
        }

        public async Task<Email> BuildEmail(EmailRequestModel emailRequest)
        {
            _logger.LogInformation("Build email");

            var template = await GetTemplate(emailRequest.TemplateType);
            template = FormatTemplate(template, emailRequest);
            var emailModel = new Email
            {
                FromName = emailRequest.SenderName,
                FromEmail = emailRequest.SenderEmail,
                To = emailRequest.DestinationAddress,
                SenderName = emailRequest.SenderName,
                Subject = EmailConstants.GetSubject(emailRequest.TemplateType),
                Content = template,
                Attachment = emailRequest.Attachment
            };

            return emailModel;
        }
        private async Task<string> GetTemplate(EmailTemplate templateType)
        {
            if (!_memoryCache.TryGetValue(templateType, out string template))
            {
                var filePath = _templateFileSelector.GetTemplatePath(templateType);
                using (var streamReader = File.OpenText(filePath))
                {
                    template = await streamReader.ReadToEndAsync();
                }

                _memoryCache.Set(templateType, template);
            }

            return template;
        }

        private static string FormatTemplate(string template, EmailRequestModel emailRequest)
        {
            foreach (var placeholder in emailRequest.PlaceholderContent)
            {
                template = template.Replace($"%%{placeholder.Key}%%", HttpUtility.HtmlEncode(placeholder.Value));
            }

            return template;
        }
    }
}
