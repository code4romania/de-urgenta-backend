using System.Threading.Tasks;
using DeUrgenta.Emailing.Service.Models;
using DeUrgenta.RecurringJobs.Domain.Entities;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders.EmailBuilders
{
    public interface IEmailRequestBuilder
    {
        Task<EmailRequestModel> CreateEmailRequest(Notification notification);
    }
}