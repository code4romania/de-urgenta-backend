using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.IdentityServer.Services.Emailing
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
            var directory = targetDirectory;
            var filePath = EmailConstants.GetTemplatePath(template);

            return Path.Combine(directory, filePath);
        }
    }
}
