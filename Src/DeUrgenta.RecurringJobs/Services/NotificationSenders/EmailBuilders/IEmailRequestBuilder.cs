using System.Threading.Tasks;
using DeUrgenta.Emailing.Service.Models;
using DeUrgenta.Domain.RecurringJobs.Entities;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders.EmailBuilders
{
    public interface IEmailRequestBuilder
    {
        Task<EmailRequestModel> CreateEmailRequest(Notification notification);
    }
}