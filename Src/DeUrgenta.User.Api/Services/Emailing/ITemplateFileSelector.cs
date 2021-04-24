namespace DeUrgenta.User.Api.Services.Emailing
{
    public interface ITemplateFileSelector
    {
        string GetTemplatePath(EmailTemplate template);
    }
}
