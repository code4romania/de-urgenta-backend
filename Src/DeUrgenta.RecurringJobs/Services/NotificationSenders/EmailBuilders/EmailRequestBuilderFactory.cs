using System;
using DeUrgenta.Domain.RecurringJobs.Entities;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders.EmailBuilders
{
    public class EmailRequestBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailRequestBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEmailRequestBuilder GetBuilderInstance(NotificationType notificationType)
        {
            return notificationType switch
            {
                NotificationType.Certification
                    => (IEmailRequestBuilder)_serviceProvider.GetService(typeof(CertificationEmailRequestBuilder)),
                var t when
                    t == NotificationType.BackpackWaterAndFood ||
                    t == NotificationType.BackpackHygiene ||
                    t == NotificationType.BackpackFirstAid ||
                    t == NotificationType.BackpackPapersAndDocuments ||
                    t == NotificationType.BackpackSurvivingArticles ||
                    t == NotificationType.BackpackRandomStuff
                    => (IEmailRequestBuilder)_serviceProvider.GetService(typeof(BackpackItemEmailRequestBuilder)),
                _ => throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null)
            };
        }
    }
}