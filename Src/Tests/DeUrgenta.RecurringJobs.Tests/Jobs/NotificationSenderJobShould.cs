using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Domain.RecurringJobs.Entities;
using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Services;
using DeUrgenta.RecurringJobs.Tests.Builders;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.RecurringJobs.Tests.Jobs
{
    [Collection(TestsConstants.DbCollectionName)]
    public class NotificationSenderJobShould
    {
        private readonly JobsContext _jobsContext;
        public NotificationSenderJobShould(JobsDatabaseFixture fixture)
        {
            _jobsContext = fixture.JobsContext;
        }

        [Fact]
        public async Task Send_notification_if_status_is_not_sent()
        {
            //Arrange
            var notification = new NotificationBuilder()
                .WithStatus(NotificationStatus.NotSent)
                .Build();

            await _jobsContext.Notifications.AddAsync(notification);
            await _jobsContext.SaveChangesAsync();

            var notificationService = Substitute.For<INotificationService>();
            var sut = new NotificationSenderJob(notificationService, _jobsContext);

            //Act
            await sut.RunAsync();

            //Assert
            await notificationService.Received().SendNotificationAsync(notification.Id);
        }

        [Fact]
        public async Task Send_notification_if_scheduled_date_has_hours()
        {
            //Arrange
            var notification = new NotificationBuilder()
                .WithScheduledDate(DateTime.Now)
                .Build();

            await _jobsContext.Notifications.AddAsync(notification);
            await _jobsContext.SaveChangesAsync();

            var notificationService = Substitute.For<INotificationService>();
            var sut = new NotificationSenderJob(notificationService, _jobsContext);

            //Act
            await sut.RunAsync();

            //Assert
            await notificationService.Received().SendNotificationAsync(notification.Id);
        }

        [Theory]
        [InlineData(NotificationStatus.Sent)]
        [InlineData(NotificationStatus.InProgress)]
        [InlineData(NotificationStatus.ErrorSending)]
        public async Task Not_send_notification_if_status_is_different_than_not_sent(NotificationStatus status)
        {
            //Arrange
            var notification = new NotificationBuilder()
                .WithStatus(status)
                .Build();

            await _jobsContext.Notifications.AddAsync(notification);
            await _jobsContext.SaveChangesAsync();

            var notificationService = Substitute.For<INotificationService>();
            var sut = new NotificationSenderJob(notificationService, _jobsContext);

            //Act
            await sut.RunAsync();

            //Assert
            await notificationService.DidNotReceive().SendNotificationAsync(notification.Id);
        }

        public static IEnumerable<object[]> ScheduledDates =>
            new List<object[]>
            {
                new object[] { DateTime.Today.AddDays(1) },
                new object[] { DateTime.Today.AddDays(-1) }
            };

        [Theory]
        [MemberData(nameof(ScheduledDates))]
        public async Task Not_send_notification_if_scheduled_date_is_not_today(DateTime scheduledDate)
        {
            //Arrange
            var notification = new NotificationBuilder()
                .WithScheduledDate(scheduledDate)
                .Build();

            await _jobsContext.Notifications.AddAsync(notification);
            await _jobsContext.SaveChangesAsync();

            var notificationService = Substitute.For<INotificationService>();
            var sut = new NotificationSenderJob(notificationService, _jobsContext);

            //Act
            await sut.RunAsync();

            //Assert
            await notificationService.DidNotReceive().SendNotificationAsync(notification.Id);
        }

        [Fact]
        public async Task Update_notification_status_to_sent_after_sending()
        {
            //Arrange
            var notification = new NotificationBuilder()
                .WithScheduledDate(DateTime.Today)
                .WithStatus(NotificationStatus.NotSent)
                .Build();

            await _jobsContext.Notifications.AddAsync(notification);
            await _jobsContext.SaveChangesAsync();

            var notificationService = Substitute.For<INotificationService>();
            notificationService.SendNotificationAsync(notification.Id).Returns(Task.CompletedTask);

            var sut = new NotificationSenderJob(notificationService, _jobsContext);

            //Act
            await sut.RunAsync();

            //Assert
            notification.Status.Should().Be(NotificationStatus.Sent);
        }
    }
}
