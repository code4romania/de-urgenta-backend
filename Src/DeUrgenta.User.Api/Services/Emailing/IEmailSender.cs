using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.User.Api.Services.Emailing
{
    public interface IEmailSender
    {
        Task SendAsync(Email email, CancellationToken cancellationToken = default);
        Task SendAsync(EmailRequestModel email, CancellationToken cancellationToken = default);
    }
}
