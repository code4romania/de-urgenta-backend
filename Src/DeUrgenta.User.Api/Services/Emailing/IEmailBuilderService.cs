using System.Threading.Tasks;

namespace DeUrgenta.User.Api.Services.Emailing
{
    public interface IEmailBuilderService
    {
        Task<Email> BuildEmail(EmailRequestModel emailModel);
    }
}
