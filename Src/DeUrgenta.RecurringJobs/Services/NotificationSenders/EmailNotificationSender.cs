using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders
{
    public class EmailNotificationSender : INotificationSender
    {
        private readonly DeUrgentaContext _context;

        public EmailNotificationSender(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task SendNotificationAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }
    }
}