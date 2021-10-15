using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Emailing.Service.Models;
using DeUrgenta.RecurringJobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.RecurringJobs.Services.NotificationSenders.EmailBuilders
{
    public class BackpackItemEmailRequestBuilder : IEmailRequestBuilder
    {
        private DeUrgentaContext _context;

        public BackpackItemEmailRequestBuilder(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<EmailRequestModel> CreateEmailRequest(Notification notification)
        {
            var user = await _context.Users.FindAsync(notification.UserId);
            var backpackItem = await _context.BackpackItems
                .Include(bi => bi.Backpack)
                .FirstOrDefaultAsync(bi => bi.Id == notification.ItemDetails.ItemId);

            var email = new EmailRequestModel
            {
                DestinationAddress = user.Email,
                TemplateType = EmailTemplate.ExpiredBackBackpackItem,
                PlaceholderContent = new Dictionary<string, string>
                {
                    { "itemName", backpackItem.Name },
                    { "backpackName", backpackItem.Backpack.Name },
                    { "itemExpirationDate", backpackItem.ExpirationDate.Value.ToString(EmailConstants.DateFormat) }
                }
            };
            return email;
        }
    }
}