using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.User.Api.Services.Emailing
{
    public abstract class BaseEmailSender : IEmailSender
    {
        private readonly IEmailBuilderService _emailBuilder;

        public BaseEmailSender(IEmailBuilderService emailBuilder)
        {
            _emailBuilder = emailBuilder;
        }

        public abstract Task SendAsync(Email email, CancellationToken cancellationToken = default);

        public async Task SendAsync(EmailRequestModel emailRequest, CancellationToken cancellationToken = default)
        {
            var email = await _emailBuilder.BuildEmail(emailRequest);

            await SendAsync(email, cancellationToken);
        }
    }
}