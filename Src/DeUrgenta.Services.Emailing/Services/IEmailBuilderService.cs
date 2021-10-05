using System.Threading.Tasks;
using DeUrgenta.Services.Emailing.Models;

namespace DeUrgenta.Services.Emailing.Services
{
    public interface IEmailBuilderService
    {
        Task<Email> BuildEmail(EmailRequestModel emailModel);
    }
}
