using DeUrgenta.Services.Emailing.Models;

namespace DeUrgenta.Services.Emailing.Services
{
    public interface ITemplateFileSelector
    {
        string GetTemplatePath(EmailTemplate template);
    }
}
