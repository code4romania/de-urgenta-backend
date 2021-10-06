using System.Threading.Tasks;
using DeUrgenta.Emailing.Service.Models;

namespace DeUrgenta.Emailing.Service.Builders
{
    public interface IEmailBuilderService
    {
        Task<Email> BuildEmail(EmailRequestModel emailModel);
    }
}
