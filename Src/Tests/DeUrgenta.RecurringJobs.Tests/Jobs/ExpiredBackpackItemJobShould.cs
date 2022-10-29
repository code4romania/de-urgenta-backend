using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.RecurringJobs.Tests.Builders;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using System.Linq;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Domain.RecurringJobs.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Jobs.Config;
using System.Threading;

namespace DeUrgenta.RecurringJobs.Tests.Jobs
{
    [Collection(TestsConstants.DbCollectionName)]
    public class ExpiredBackpackItemJobShould
    {
        private readonly DeUrgentaContext _context;
        private readonly JobsContext _jobsContext;

        public ExpiredBackpackItemJobShould(JobsDatabaseFixture fixture)
        {
            _context = fixture.Context;
            _jobsContext = fixture.JobsContext;
        }

        [Theory]
        [InlineData(10)]
        [InlineData(9)]
        public async Task Trigger_notification_when_item_expires_before_or_on_the_day_the_job_runs(uint daysAfterCurrentDate)
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            _context.Users.Add(user);

            var backpackItem = new BackpackItemBuilder()
                .WithExpirationDate(DateTime.Today.AddDays(daysAfterCurrentDate))
                .WithCategory(BackpackItemCategoryType.WaterAndFood)
                .Build();

            _context.BackpackItems.Add(backpackItem);

            var backpackToUser = new BackpackToUserBuilder().WithUser(user).WithBackpack(backpackItem.Backpack).Build();
            _context.BackpacksToUsers.Add(backpackToUser);

            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredBackpackItemJobConfig>>();
            jobConfig.Value.Returns(new ExpiredBackpackItemJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredBackpackItemJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications.Where(n => n.UserId == userId).ToList();
            notificationsAdded.Should().BeEquivalentTo(new List<Notification>
            {
                new()
                {
                    ItemDetails = new ItemDetails{ ItemId = backpackItem.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.BackpackWaterAndFood,
                    ScheduledDate = DateTime.Today
                },
                new()
                {
                    ItemDetails = new ItemDetails{ ItemId = backpackItem.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.BackpackWaterAndFood,
                    ScheduledDate = backpackItem.ExpirationDate.Value
                }
            }, opt => opt.Excluding(n => n.Id)
                .Excluding(n => n.ItemDetails.Notification)
                .Excluding(n => n.ItemDetails.NotificationId));

        }

        [Fact]
        public async Task Not_trigger_notification_when_item_expires_after_the_day_the_job_runs()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            _context.Users.Add(user);

            var backpackItem = new BackpackItemBuilder()
                .WithExpirationDate(DateTime.Today.AddDays(11))
                .Build();
            _context.BackpackItems.Add(backpackItem);

            var backpackToUser = new BackpackToUserBuilder().WithUser(user).WithBackpack(backpackItem.Backpack).Build();
            _context.BackpacksToUsers.Add(backpackToUser);

            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredBackpackItemJobConfig>>();
            jobConfig.Value.Returns(new ExpiredBackpackItemJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredBackpackItemJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications.Where(n => n.UserId == userId).ToList();
            notificationsAdded.Should().BeEmpty();
        }

        [Fact]
        public async Task Not_trigger_notification_when_item_already_expired()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            _context.Users.Add(user);

            var backpackItem = new BackpackItemBuilder()
                .WithExpirationDate(DateTime.Today.AddDays(11))
                .Build();
            _context.BackpackItems.Add(backpackItem);

            var backpackToUser = new BackpackToUserBuilder().WithUser(user).WithBackpack(backpackItem.Backpack).Build();
            _context.BackpacksToUsers.Add(backpackToUser);

            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredBackpackItemJobConfig>>();
            jobConfig.Value.Returns(new ExpiredBackpackItemJobConfig
            {
                DaysBeforeExpirationDate = 10
            });

            var sut = new ExpiredBackpackItemJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications.Where(n => n.UserId == userId).ToList();
            notificationsAdded.Should().BeEmpty();
        }

        [Fact]
        public async Task Not_trigger_notification_when_a_not_sent_notification_already_exists()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            _context.Users.Add(user);

            var backpackItem = new BackpackItemBuilder()
                .WithExpirationDate(DateTime.Today.AddDays(8))
                .Build();
            _context.BackpackItems.Add(backpackItem);

            var backpackToUser = new BackpackToUserBuilder().WithUser(user).WithBackpack(backpackItem.Backpack).Build();
            _context.BackpacksToUsers.Add(backpackToUser);

            var notification = new NotificationBuilder()
                .WithScheduledDate(DateTime.Today.AddDays(1))
                .WithUserId(userId)
                .WithItemId(backpackItem.Id)
                .WithStatus(NotificationStatus.NotSent)
                .Build();

            _jobsContext.Add(notification);

            await _context.SaveChangesAsync();
            await _jobsContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredBackpackItemJobConfig>>();
            jobConfig.Value.Returns(new ExpiredBackpackItemJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredBackpackItemJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications
                .Where(n => n.UserId == userId && n.Id != notification.Id)
                .ToList();
            notificationsAdded.Should().BeEmpty();
        }

        [Fact]
        public async Task Not_trigger_notification_when_item_has_no_expiration_date()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            _context.Users.Add(user);

            var backpackItem = new BackpackItemBuilder()
                .WithExpirationDate(null)
                .Build();
            _context.BackpackItems.Add(backpackItem);

            var backpackToUser = new BackpackToUserBuilder().WithUser(user).WithBackpack(backpackItem.Backpack).Build();
            _context.BackpacksToUsers.Add(backpackToUser);

            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredBackpackItemJobConfig>>();
            jobConfig.Value.Returns(new ExpiredBackpackItemJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredBackpackItemJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications.Where(n => n.UserId == userId).ToList();
            notificationsAdded.Should().BeEmpty();
        }

        [Fact]
        public async Task Trigger_notification_when_a_notification_for_a_different_item_exists()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            _context.Users.Add(user);

            var backpackItem = new BackpackItemBuilder()
                .WithExpirationDate(DateTime.Today.AddDays(8))
                .Build();
            _context.BackpackItems.Add(backpackItem);

            var backpackToUser = new BackpackToUserBuilder().WithUser(user).WithBackpack(backpackItem.Backpack).Build();
            _context.BackpacksToUsers.Add(backpackToUser);

            var notification = new NotificationBuilder()
                .WithScheduledDate(DateTime.Today.AddDays(1))
                .WithUserId(userId)
                .WithItemId(Guid.NewGuid())
                .Build();

            _jobsContext.Add(notification);

            await _context.SaveChangesAsync();
            await _jobsContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredBackpackItemJobConfig>>();
            jobConfig.Value.Returns(new ExpiredBackpackItemJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredBackpackItemJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications
                .Where(n => n.UserId == userId && n.Id != notification.Id)
                .ToList();
            notificationsAdded.Should().BeEquivalentTo(new List<Notification>
            {
                new()
                {
                    ItemDetails = new ItemDetails{ ItemId = backpackItem.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.BackpackFirstAid,
                    ScheduledDate = DateTime.Today
                },
                new()
                {
                    ItemDetails = new ItemDetails{ ItemId = backpackItem.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.BackpackFirstAid,
                    ScheduledDate = backpackItem.ExpirationDate.Value
                }
            }, opt => opt.Excluding(n => n.Id)
                .Excluding(n => n.ItemDetails.Notification)
                .Excluding(n => n.ItemDetails.NotificationId));
        }

        [Theory]
        [InlineData(NotificationStatus.Sent)]
        [InlineData(NotificationStatus.NotSent)]
        public async Task Trigger_notification_when_a_past_notification_exists(NotificationStatus status)
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            _context.Users.Add(user);

            var backpackItem = new BackpackItemBuilder()
                .WithExpirationDate(DateTime.Today.AddDays(8))
                .Build();
            _context.BackpackItems.Add(backpackItem);

            var backpackToUser = new BackpackToUserBuilder().WithUser(user).WithBackpack(backpackItem.Backpack).Build();
            _context.BackpacksToUsers.Add(backpackToUser);

            var notification = new NotificationBuilder()
                .WithScheduledDate(DateTime.Today.AddDays(-1))
                .WithUserId(userId)
                .WithItemId(backpackItem.Id)
                .WithStatus(status)
                .Build();

            _jobsContext.Add(notification);

            await _context.SaveChangesAsync();
            await _jobsContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredBackpackItemJobConfig>>();
            jobConfig.Value.Returns(new ExpiredBackpackItemJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredBackpackItemJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications
                .Where(n => n.UserId == userId && n.Id != notification.Id).ToList();
            notificationsAdded.Should().BeEquivalentTo(new List<Notification>
            {
                new()
                {
                    ItemDetails = new ItemDetails{ ItemId = backpackItem.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.BackpackFirstAid,
                    ScheduledDate = DateTime.Today
                },
                new() 
                {
                    ItemDetails = new ItemDetails{ ItemId = backpackItem.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.BackpackFirstAid,
                    ScheduledDate = backpackItem.ExpirationDate.Value
                }
            }, opt => opt.Excluding(n => n.Id)
                .Excluding(n => n.ItemDetails.Notification)
                .Excluding(n => n.ItemDetails.NotificationId));
        }

        [Theory]
        [InlineData(BackpackItemCategoryType.FirstAid, NotificationType.BackpackFirstAid)]
        [InlineData(BackpackItemCategoryType.Hygiene, NotificationType.BackpackHygiene)]
        [InlineData(BackpackItemCategoryType.PapersAndDocuments, NotificationType.BackpackPapersAndDocuments)]
        [InlineData(BackpackItemCategoryType.RandomStuff, NotificationType.BackpackRandomStuff)]
        [InlineData(BackpackItemCategoryType.SurvivingArticles, NotificationType.BackpackSurvivingArticles)]
        [InlineData(BackpackItemCategoryType.WaterAndFood, NotificationType.BackpackWaterAndFood)]
        public async Task Trigger_notification_with_Correct_type(BackpackItemCategoryType backpackCategoryType, NotificationType expectedType)
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            _context.Users.Add(user);

            var backpackItem = new BackpackItemBuilder()
                .WithExpirationDate(DateTime.Today.AddDays(8))
                .WithCategory(backpackCategoryType)
                .Build();
            _context.BackpackItems.Add(backpackItem);

            var backpackToUser = new BackpackToUserBuilder().WithUser(user).WithBackpack(backpackItem.Backpack).Build();
            _context.BackpacksToUsers.Add(backpackToUser);

            await _context.SaveChangesAsync();
            await _jobsContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredBackpackItemJobConfig>>();
            jobConfig.Value.Returns(new ExpiredBackpackItemJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredBackpackItemJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications.Where(n => n.UserId == userId).ToList();
            notificationsAdded.Select(n => n.Type)
                .Distinct()
                .Should()
                .BeEquivalentTo(new List<NotificationType> { expectedType });
        }
    }
}
