namespace DeUrgenta.IdentityServer.Services.Emailing
{
    public interface ITemplateFileSelector
    {
        string GetTemplatePath(EmailTemplate template);
    }
}
