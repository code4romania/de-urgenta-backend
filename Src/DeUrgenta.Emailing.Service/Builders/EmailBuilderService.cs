using System.IO;
using System.Threading.Tasks;
using System.Web;
using DeUrgenta.Emailing.Service.Config;
using DeUrgenta.Emailing.Service.Constants;
using DeUrgenta.Emailing.Service.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Emailing.Service.Builders
{
    public class EmailBuilderService : IEmailBuilderService
    {
        private readonly EmailConfigOptions _configOptions;
        private readonly ILogger<IEmailBuilderService> _logger;
        private readonly IMemoryCache _memoryCache;

        public EmailBuilderService(ILogger<IEmailBuilderService> logger, IMemoryCache memoryCache, IOptions<EmailConfigOptions> options)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _configOptions = options.Value;
        }

        public async Task<Email> BuildEmail(EmailRequestModel emailRequest)
        {
            _logger.LogInformation("Build email");

            var template = await GetTemplate(emailRequest.TemplateType);
            template = FormatTemplate(template, emailRequest);
            var emailModel = new Email
            {
                FromName = _configOptions.AdminFromName,
                FromEmail = _configOptions.AdminFromEmail,
                To = emailRequest.DestinationAddress,
                SenderName = _configOptions.AdminFromName,
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
                var filePath = GetTemplatePath(templateType);
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

        public string GetTemplatePath(EmailTemplate template)
        {
            var targetDirectory = _configOptions.TemplateFolder;
            var filePath = EmailConstants.GetTemplatePath(template);

            return Path.Combine(targetDirectory, filePath);
        }
    }
}
