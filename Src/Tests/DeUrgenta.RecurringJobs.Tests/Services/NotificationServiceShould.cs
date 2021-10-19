using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.Domain.RecurringJobs.Entities;
using DeUrgenta.RecurringJobs.Services;
using DeUrgenta.RecurringJobs.Services.NotificationSenders;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace DeUrgenta.RecurringJobs.Tests.Services
{
    public class NotificationServiceShould
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly List<INotificationSender> _senders;
        private readonly INotificationSender _emailNotificationSender;
        private readonly INotificationSender _pushNotificationSender;

        public NotificationServiceShould()
        {
            _logger = Substitute.For<ILogger<NotificationService>>();
            _emailNotificationSender = Substitute.For<INotificationSender>();
            _pushNotificationSender = Substitute.For<INotificationSender>();
            _senders = new List<INotificationSender> { _emailNotificationSender, _pushNotificationSender };
        }

        [Fact]
        public async Task Return_not_sent_if_there_are_no_senders_configured()
        {
            //Arrange
            var sut = new NotificationService(null, _logger);

            //Act
            var result = await sut.SendNotificationAsync(Guid.NewGuid());

            //Assert
            result.Should().Be(NotificationStatus.NotSent);
        }

        [Fact]
        public async Task Return_sent_if_all_senders_were_successful()
        {
            //Arrange
            var notificationId = Guid.NewGuid();
            _emailNotificationSender.SendNotificationAsync(notificationId).Returns(true);
            _pushNotificationSender.SendNotificationAsync(notificationId).Returns(true);
            var sut = new NotificationService(_senders, _logger);

            //Act
            var result = await sut.SendNotificationAsync(notificationId);

            //Assert
            result.Should().Be(NotificationStatus.Sent);
        }

        [Fact]
        public async Task Return_error_if_all_senders_failed()
        {
            //Arrange
            var notificationId = Guid.NewGuid();
            _emailNotificationSender.SendNotificationAsync(notificationId).Returns(false);
            _pushNotificationSender.SendNotificationAsync(notificationId).Returns(false);
            var sut = new NotificationService(_senders, _logger);

            //Act
            var result = await sut.SendNotificationAsync(notificationId);

            //Assert
            result.Should().Be(NotificationStatus.ErrorSending);
        }

        [Fact]
        public async Task Return_partly_sent_if_only_some_senders_failed()
        {
            //Arrange
            var notificationId = Guid.NewGuid();
            _emailNotificationSender.SendNotificationAsync(notificationId).Returns(false);
            _pushNotificationSender.SendNotificationAsync(notificationId).Returns(true);
            var sut = new NotificationService(_senders, _logger);

            //Act
            var result = await sut.SendNotificationAsync(notificationId);

            //Assert
            result.Should().Be(NotificationStatus.PartlySent);
        }
    }
}
