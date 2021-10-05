using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Services.Emailing.Models;

namespace DeUrgenta.Services.Emailing.Services
{
    public interface IEmailSender
    {
        Task SendAsync(Email email, CancellationToken cancellationToken = default);
        Task SendAsync(EmailRequestModel email, CancellationToken cancellationToken = default);
    }
}
