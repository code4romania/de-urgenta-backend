using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace DeUrgenta.RecurringJobs.Tests.Jobs
{
    [Collection(TestsConstants.DbCollectionName)]
    public class EventArchivalJobShould
    {
        private readonly DeUrgentaContext _context;

        public EventArchivalJobShould(JobsDatabaseFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Archive_event_when_the_event_is_due()
        {
            //Arrange
            var eventType = new EventType { Name = "TestType" };
            eventType = _context.EventTypes.Add(eventType).Entity;
            await _context.SaveChangesAsync();

            var eventEntity = new EventBuilder()
                .WithEventTypeId(eventType.Id)
                .WithDate(DateTime.UnixEpoch)
                .Build();

            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<EventArchivalJobConfig>>();
            jobConfig.CurrentValue.Returns(new EventArchivalJobConfig());

            var sut = new EventArchivalJob(_context);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert

            eventEntity.IsArchived.Should().BeTrue();
        }

        [Fact]
        public async Task Do_not_archive_event_when_the_event_is_due()
        {
            //Arrange
            var eventType = new EventType { Name = "TestType" };
            eventType = _context.EventTypes.Add(eventType).Entity;
            await _context.SaveChangesAsync();

            var eventEntity = new EventBuilder()
                .WithEventTypeId(eventType.Id)
                .WithDate(DateTime.Today.AddDays(1))
                .Build();

            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<EventArchivalJobConfig>>();
            jobConfig.CurrentValue.Returns(new EventArchivalJobConfig());

            var sut = new EventArchivalJob(_context);

            //Act
            await sut.RunAsync(CancellationToken.None);

            //Assert
            eventEntity.IsArchived.Should().BeFalse();
        }
    }
}
