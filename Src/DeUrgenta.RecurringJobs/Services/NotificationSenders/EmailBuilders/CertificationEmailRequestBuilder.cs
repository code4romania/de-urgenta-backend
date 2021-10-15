using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Emailing.Service.Models;
using DeUrgenta.RecurringJobs.Domain.Entities;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders.EmailBuilders
{
    public class CertificationEmailRequestBuilder : IEmailRequestBuilder
    {
        private DeUrgentaContext _context;

        public CertificationEmailRequestBuilder(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<EmailRequestModel> CreateEmailRequest(Notification notification)
        {
            var user = await _context.Users.FindAsync(notification.UserId);
            var certification = await _context.Certifications.FindAsync(notification.CertificationDetails.CertificationId);

            var email = new EmailRequestModel
            {
                DestinationAddress = user.Email,
                TemplateType = EmailTemplate.ExpiredCertification,
                PlaceholderContent = new Dictionary<string, string>
                {
                    { "certificationName", certification.Name },
                    { "certificationExpirationDate", certification.ExpirationDate.ToString(EmailConstants.DateFormat) }
                }
            };
            return email;
        }
    }
}