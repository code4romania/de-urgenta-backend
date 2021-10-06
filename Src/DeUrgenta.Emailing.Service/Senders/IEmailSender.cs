using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Emailing.Service.Models;

namespace DeUrgenta.Emailing.Service.Senders
{
    public interface IEmailSender
    {
        Task SendAsync(Email email, CancellationToken cancellationToken = default);
        Task SendAsync(EmailRequestModel email, CancellationToken cancellationToken = default);
    }
}
