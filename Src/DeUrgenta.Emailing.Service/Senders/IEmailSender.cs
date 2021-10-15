using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Emailing.Service.Models;

namespace DeUrgenta.Emailing.Service.Senders
{
    public interface IEmailSender
    {
        Task<bool> SendAsync(Email email, CancellationToken cancellationToken = default);
        Task<bool> SendAsync(EmailRequestModel email, CancellationToken cancellationToken = default);
    }
}
