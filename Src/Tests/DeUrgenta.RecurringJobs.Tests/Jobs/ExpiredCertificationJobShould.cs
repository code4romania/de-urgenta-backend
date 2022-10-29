using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.RecurringJobs;
using DeUrgenta.Domain.RecurringJobs.Entities;
using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.RecurringJobs.Tests.Builders;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace DeUrgenta.RecurringJobs.Tests.Jobs
{
    [Collection(TestsConstants.DbCollectionName)]
    public class ExpiredCertificationJobShould
    {
        private readonly DeUrgentaContext _context;
        private readonly JobsContext _jobsContext;

        public ExpiredCertificationJobShould(JobsDatabaseFixture fixture)
        {
            _context = fixture.Context;
            _jobsContext = fixture.JobsContext;
        }

        [Theory]
        [InlineData(10)]
        [InlineData(9)]
        public async Task Trigger_notification_when_certification_expires_before_or_on_the_day_the_job_runs(uint daysAfterCurrentDate)
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            var certification = new CertificationBuilder()
                .WithUserId(userId)
                .WithExpirationDate(DateTime.Today.AddDays(daysAfterCurrentDate))
                .Build();

            _context.Users.Add(user);
            _context.Certifications.Add(certification);
            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<ExpiredCertificationJobConfig>>();
            jobConfig.CurrentValue.Returns(new ExpiredCertificationJobConfig
            {
                DaysBeforeExpirationDate = 10
            });

            var sut = new ExpiredCertificationJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications.Where(n => n.UserId == userId).ToList();
            notificationsAdded.Should().BeEquivalentTo(new List<Notification>
            {
                new()
                {
                    CertificationDetails = new CertificationDetails{ CertificationId = certification.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.Certification,
                    ScheduledDate = DateTime.Today
                },
                new()
                {
                    CertificationDetails = new CertificationDetails{ CertificationId = certification.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.Certification,
                    ScheduledDate = certification.ExpirationDate
                }
            }, opt => opt.Excluding(n => n.Id)
                .Excluding(n => n.CertificationDetails.Notification)
                .Excluding(n => n.CertificationDetails.NotificationId));

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

            _context.Users.Add(user);
            _context.Certifications.Add(certification);
            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<ExpiredCertificationJobConfig>>();
            jobConfig.CurrentValue.Returns(new ExpiredCertificationJobConfig
            {
                DaysBeforeExpirationDate = 10
            });

            var sut = new ExpiredCertificationJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications.Where(n => n.UserId == userId).ToList();
            notificationsAdded.Should().BeEmpty();
        }

        [Fact]
        public async Task Not_trigger_notification_when_certification_already_expired()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            var certification = new CertificationBuilder()
                .WithUserId(userId)
                .WithExpirationDate(DateTime.Today.AddDays(-11))
                .Build();

            _context.Users.Add(user);
            _context.Certifications.Add(certification);
            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<ExpiredCertificationJobConfig>>();
            jobConfig.CurrentValue.Returns(new ExpiredCertificationJobConfig
            {
                DaysBeforeExpirationDate = 10
            });

            var sut = new ExpiredCertificationJob(_context, _jobsContext, jobConfig);

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
            var certification = new CertificationBuilder()
                .WithUserId(userId)
                .WithExpirationDate(DateTime.Today.AddDays(8))
                .Build();

            _context.Users.Add(user);
            _context.Certifications.Add(certification);

            var notification = new NotificationBuilder()
                .WithScheduledDate(DateTime.Today.AddDays(1))
                .WithUserId(userId)
                .WithCertificationId(certification.Id)
                .WithStatus(NotificationStatus.NotSent)
                .Build();

            _jobsContext.Add(notification);

            await _context.SaveChangesAsync();
            await _jobsContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<ExpiredCertificationJobConfig>>();
            jobConfig.CurrentValue.Returns(new ExpiredCertificationJobConfig
            {
                DaysBeforeExpirationDate = 10
            });

            var sut = new ExpiredCertificationJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications
                .Where(n => n.UserId == userId && n.Id != notification.Id)
                .ToList();
            notificationsAdded.Should().BeEmpty();
        }

        [Fact]
        public async Task Trigger_notification_when_a_notification_for_a_different_certification_exists()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            var certification = new CertificationBuilder()
                .WithUserId(userId)
                .WithExpirationDate(DateTime.Today.AddDays(8))
                .Build();

            _context.Users.Add(user);
            _context.Certifications.Add(certification);

            var notification = new NotificationBuilder()
                .WithScheduledDate(DateTime.Today.AddDays(1))
                .WithUserId(userId)
                .WithCertificationId(Guid.NewGuid())
                .Build();

            _jobsContext.Add(notification);

            await _context.SaveChangesAsync();
            await _jobsContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<ExpiredCertificationJobConfig>>();
            jobConfig.CurrentValue.Returns(new ExpiredCertificationJobConfig
            {
                DaysBeforeExpirationDate = 10
            });

            var sut = new ExpiredCertificationJob(_context, _jobsContext, jobConfig);

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
                    CertificationDetails = new CertificationDetails{ CertificationId = certification.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.Certification,
                    ScheduledDate = DateTime.Today
                },
                new()
                {
                    CertificationDetails = new CertificationDetails{ CertificationId = certification.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.Certification,
                    ScheduledDate = certification.ExpirationDate
                }
            }, opt => opt.Excluding(n => n.Id)
                .Excluding(n => n.CertificationDetails.Notification)
                .Excluding(n => n.CertificationDetails.NotificationId));
        }

        [Theory]
        [InlineData(NotificationStatus.Sent)]
        [InlineData(NotificationStatus.NotSent)]
        public async Task Trigger_notification_when_a_past_notification_exists(NotificationStatus status)
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();
            var certification = new CertificationBuilder()
                .WithUserId(userId)
                .WithExpirationDate(DateTime.Today.AddDays(8))
                .Build();

            _context.Users.Add(user);
            _context.Certifications.Add(certification);

            var notification = new NotificationBuilder()
                .WithScheduledDate(DateTime.Today.AddDays(-1))
                .WithUserId(userId)
                .WithCertificationId(certification.Id)
                .WithStatus(status)
                .Build();

            _jobsContext.Add(notification);

            await _context.SaveChangesAsync();
            await _jobsContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<ExpiredCertificationJobConfig>>();
            jobConfig.CurrentValue.Returns(new ExpiredCertificationJobConfig
            {
                DaysBeforeExpirationDate = 10
            });

            var sut = new ExpiredCertificationJob(_context, _jobsContext, jobConfig);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            var notificationsAdded = _jobsContext.Notifications
                .Where(n => n.UserId == userId && n.Id != notification.Id).ToList();
            notificationsAdded.Should().BeEquivalentTo(new List<Notification>
            {
                new()
                {
                    CertificationDetails = new CertificationDetails{ CertificationId = certification.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.Certification,
                    ScheduledDate = DateTime.Today
                },
                new()
                {
                    CertificationDetails = new CertificationDetails{ CertificationId = certification.Id },
                    UserId = userId,
                    Status = NotificationStatus.NotSent,
                    Type = NotificationType.Certification,
                    ScheduledDate = certification.ExpirationDate
                }
            }, opt => opt.Excluding(n => n.Id)
                .Excluding(n => n.CertificationDetails.Notification)
                .Excluding(n => n.CertificationDetails.NotificationId));
        }
    }
}
