using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.RecurringJobs.Services;
using Microsoft.Extensions.Options;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class ExpiredCertificationJob : IExpiredCertificationJob
    {
        private readonly DeUrgentaContext _context;
        private readonly INotificationService _notificationService;
        private readonly ExpiredCertificationJobConfig _config;

        public ExpiredCertificationJob(DeUrgentaContext context, INotificationService notificationService, IOptionsMonitor<ExpiredCertificationJobConfig> config)
        {
            _context = context;
            _notificationService = notificationService;
            _config = config.CurrentValue;
        }

        public async Task RunAsync()
        {
            var usersWithExpiredCertifications = _context.Certifications
                .Where(c => (c.ExpirationDate - DateTime.Today).Days <= _config.DaysBeforeExpirationDate)
                .Select(c => c.UserId)
                .ToList();

            foreach (var userId in usersWithExpiredCertifications)
            {
                await _notificationService.SendNotificationAsync(userId);
            }
        }
    }
}
