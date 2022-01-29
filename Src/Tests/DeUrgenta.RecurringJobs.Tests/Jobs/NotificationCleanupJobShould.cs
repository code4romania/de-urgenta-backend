using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Domain.RecurringJobs.Entities;
using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Tests.Builders;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.RecurringJobs.Tests.Jobs
{
    [Collection(TestsConstants.DbCollectionName)]
    public class NotificationCleanupJobShould
    {
        private readonly JobsContext _jobsContext;

        public NotificationCleanupJobShould(JobsDatabaseFixture fixture)
        {
            _jobsContext = fixture.JobsContext;
        }

        [Fact]
        public async Task Delete_sent_notifications()
        {
            //Arrange
            var notification = new NotificationBuilder()
                .WithStatus(NotificationStatus.Sent)
                .Build();

            await _jobsContext.Notifications.AddAsync(notification);
            await _jobsContext.SaveChangesAsync();

            var sut = new NotificationCleanupJob(_jobsContext);

            //Act
            await sut.RunAsync();

            //Assert
            var deletedNotification = _jobsContext.Notifications.FirstOrDefault(n => n.Id == notification.Id);
            deletedNotification.Should().BeNull();
        }

        [Theory]
        [InlineData(NotificationStatus.ErrorSending)]
        [InlineData(NotificationStatus.InProgress)]
        [InlineData(NotificationStatus.NotSent)]
        public async Task Not_delete_notifications_with_different_status(NotificationStatus status)
        {
            //Arrange
            var notification = new NotificationBuilder()
                .WithStatus(status)
                .Build();

            await _jobsContext.Notifications.AddAsync(notification);
            await _jobsContext.SaveChangesAsync();

            var sut = new NotificationCleanupJob(_jobsContext);

            //Act
            await sut.RunAsync();

            //Assert
            var deletedNotification = _jobsContext.Notifications.FirstOrDefault(n => n.Id == notification.Id);
            deletedNotification.Should().NotBeNull();
        }
    }
}
