using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Domain.RecurringJobs.Entities;
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
                    Type = GetNotificationType(expiringItem.Category),
                    ScheduledDate = computedNotificationDate < DateTime.Today ? DateTime.Today : computedNotificationDate,
                    ItemDetails = new ItemDetails { ItemId = expiringItem.Id },
                    UserId = backpackUser.UserId,
                    Status = NotificationStatus.NotSent
                };
                var expirationDayNotification = new Notification
                {
                    Type = GetNotificationType(expiringItem.Category),
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

        private NotificationType GetNotificationType(BackpackItemCategoryType backpackItemCategoryType)
        {

            return backpackItemCategoryType switch
            {
                BackpackItemCategoryType.WaterAndFood => NotificationType.BackpackWaterAndFood,
                BackpackItemCategoryType.FirstAid => NotificationType.BackpackFirstAid,
                BackpackItemCategoryType.Hygiene => NotificationType.BackpackHygiene,
                BackpackItemCategoryType.SurvivingArticles => NotificationType.BackpackSurvivingArticles,
                BackpackItemCategoryType.PapersAndDocuments => NotificationType.BackpackPapersAndDocuments,
                BackpackItemCategoryType.RandomStuff => NotificationType.BackpackRandomStuff,
                _ => throw new ArgumentOutOfRangeException(nameof(backpackItemCategoryType), backpackItemCategoryType, null)
            };
        }
    }
}
