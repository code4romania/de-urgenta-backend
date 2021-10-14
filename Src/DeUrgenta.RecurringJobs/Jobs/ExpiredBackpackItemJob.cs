using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.RecurringJobs.Domain;
using DeUrgenta.RecurringJobs.Domain.Entities;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.RecurringJobs.Jobs.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.RecurringJobs.Jobs
{
    public class ExpiredBackpackItemJob : IExpiredBackpackItemJob
    {
        private DeUrgentaContext _context;
        private JobsContext _jobsContext;

        private ExpiredBackpackItemJobConfig _config;

        public ExpiredBackpackItemJob(DeUrgentaContext context, JobsContext jobsContext, IOptions<ExpiredBackpackItemJobConfig> config)
        {
            _context = context;
            _jobsContext = jobsContext;
            _config = config.Value;
        }

        public async Task RunAsync()
        {
            var expiredItems = await _context.BackpackItems
                .Where(b => b.ExpirationDate != null
                    && (b.ExpirationDate.Value - DateTime.Today).Days <= _config.DaysBeforeExpirationDate
                    && b.ExpirationDate.Value >= DateTime.Today)
                .ToListAsync();

            foreach (var expiringItem in expiredItems)
            {
                var scheduledNotifications = await _jobsContext.Notifications
                    .Where(n => n.ItemDetails.ItemId == expiringItem.Id
                                && n.ScheduledDate >= DateTime.Today)
                    .Select(n => n.ItemDetails.ItemId)
                    .ToListAsync();

                if (scheduledNotifications.Count != 0)
                {
                    continue;
                }

                var backpackUser = await _context.BackpackItems
                    .Join(_context.BackpacksToUsers,
                        bi => bi.BackpackId,
                        bu => bu.BackpackId,
                        (bi, bu) => new { bu.UserId, bi.Id })
                    .FirstOrDefaultAsync(b => b.Id == expiringItem.Id);


                var computedNotificationDate = expiringItem.ExpirationDate.Value.AddDays(-_config.DaysBeforeExpirationDate);
                var preExpirationNotification = new Notification
                {
                    Type = GetNotificationType(expiringItem.BackpackCategory),
                    ScheduledDate = computedNotificationDate < DateTime.Today ? DateTime.Today : computedNotificationDate,
                    ItemDetails = new ItemDetails { ItemId = expiringItem.Id },
                    UserId = backpackUser.UserId,
                    Status = NotificationStatus.NotSent
                };
                var expirationDayNotification = new Notification
                {
                    Type = GetNotificationType(expiringItem.BackpackCategory),
                    ScheduledDate = expiringItem.ExpirationDate.Value,
                    ItemDetails = new ItemDetails { ItemId = expiringItem.Id },
                    UserId = backpackUser.UserId,
                    Status = NotificationStatus.NotSent
                };

                await _jobsContext.AddAsync(preExpirationNotification);
                await _jobsContext.AddAsync(expirationDayNotification);
                await _jobsContext.SaveChangesAsync();
            }
        }

        private NotificationType GetNotificationType(BackpackCategoryType expiringItemBackpackCategory)
        {

            return expiringItemBackpackCategory switch
            {
                BackpackCategoryType.WaterAndFood => NotificationType.BackpackWaterAndFood,
                BackpackCategoryType.FirstAid => NotificationType.BackpackFirstAid,
                BackpackCategoryType.Hygiene => NotificationType.BackpackHygiene,
                BackpackCategoryType.SurvivingArticles => NotificationType.BackpackSurvivingArticles,
                BackpackCategoryType.PapersAndDocuments => NotificationType.BackpackPapersAndDocuments,
                BackpackCategoryType.RandomStuff => NotificationType.BackpackRandomStuff,
                _ => throw new ArgumentOutOfRangeException(nameof(expiringItemBackpackCategory), expiringItemBackpackCategory, null)
            };
        }
    }
}
