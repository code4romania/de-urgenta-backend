using System.IO;
using DeUrgenta.Services.Emailing.Constants;
using DeUrgenta.Services.Emailing.Models;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.Services.Emailing.Services
{
    public class TemplateFileSelector : ITemplateFileSelector
    {
        private readonly IConfiguration _configuration;

        public TemplateFileSelector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetTemplatePath(EmailTemplate template)
        {
            var targetDirectory = _configuration.GetValue<string>("TemplateFolder");
            var filePath = EmailConstants.GetTemplatePath(template);

            return Path.Combine(targetDirectory, filePath);
        }
    }
}
