using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.RecurringJobs.Services;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace DeUrgenta.RecurringJobs.Tests.Jobs
{
    [Collection(TestsConstants.DbCollectionName)]
    public class ExpiredCertificationJobShould
    {
        private readonly DeUrgentaContext _context;

        public ExpiredCertificationJobShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;
        }

        [Theory]
        [InlineData(10)]
        [InlineData(9)]
        public async Task Trigger_notification_when_certification_expires_before_or_on_the_day_the_job_runs(uint daysBeforeExpirationDate)
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            var certification = new CertificationBuilder()
                .WithUserId(userId)
                .WithExpirationDate(DateTime.Today.AddDays(daysBeforeExpirationDate))
                .Build();

            await _context.Users.AddAsync(user);
            await _context.Certifications.AddAsync(certification);
            await _context.SaveChangesAsync();

            var notificationService = Substitute.For<INotificationService>();
            var jobConfig = Substitute.For<IOptionsMonitor<ExpiredCertificationJobConfig>>();
            jobConfig.CurrentValue.Returns(new ExpiredCertificationJobConfig
            {
                DaysBeforeExpirationDate = 10
            });

            var sut = new ExpiredCertificationJob(_context, notificationService, jobConfig);

            //Act
            sut.Run();

            //Assert
            notificationService.Received().SendNotification(userId);
        }
        
        [Fact]
        public async Task Not_trigger_notification_when_certification_expires_after_the_day_the_job_runs()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            var certification = new CertificationBuilder()
                .WithUserId(userId)
                .WithExpirationDate(DateTime.Today.AddDays(11))
                .Build();

            await _context.Users.AddAsync(user);
            await _context.Certifications.AddAsync(certification);
            await _context.SaveChangesAsync();

            var notificationService = Substitute.For<INotificationService>();
            var jobConfig = Substitute.For<IOptionsMonitor<ExpiredCertificationJobConfig>>();
            jobConfig.CurrentValue.Returns(new ExpiredCertificationJobConfig
            {
                DaysBeforeExpirationDate = 10
            });

            var sut = new ExpiredCertificationJob(_context, notificationService, jobConfig);

            //Act
            sut.Run();

            //Assert
            notificationService.DidNotReceive().SendNotification(userId);
        }
    }
}
