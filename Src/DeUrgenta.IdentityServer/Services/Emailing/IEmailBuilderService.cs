using System.Threading.Tasks;

namespace DeUrgenta.IdentityServer.Services.Emailing
{
    public interface IEmailBuilderService
    {
        Task<Email> BuildEmail(EmailRequestModel emailModel);
    }
}
