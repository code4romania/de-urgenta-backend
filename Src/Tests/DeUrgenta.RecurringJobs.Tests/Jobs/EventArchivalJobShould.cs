using System;
using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Domain.RecurringJobs;
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
        private readonly JobsContext _jobsContext;

        public EventArchivalJobShould(JobsDatabaseFixture fixture)
        {
            _context = fixture.Context;
            _jobsContext = fixture.JobsContext;
        }

        [Fact]
        public async Task Archive_event_when_the_event_is_due()
        {
            //Arrange
            var eventType = new EventType {Name = "TestType"};
            eventType = _context.EventTypes.AddAsync(eventType).Result.Entity;
            await _context.SaveChangesAsync();
            
            var eventEntity = new EventBuilder()
                .WithEventTypeId(eventType.Id)
                .WithDate(DateTime.UnixEpoch)
                .Build();
            
            var eventEntry = await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<EventArchivalJobConfig>>();
            jobConfig.CurrentValue.Returns(new EventArchivalJobConfig());

            var sut = new EventArchivalJob(_context, _jobsContext);

            //Act
            await sut.RunAsync();

            //Assert

            eventEntity.IsArchived.Should().BeTrue();
        }

        [Fact]
        public async Task Do_not_archive_event_when_the_event_is_due()
        {
            //Arrange
            var eventType = new EventType {Name = "TestType"};
            eventType = _context.EventTypes.AddAsync(eventType).Result.Entity;
            await _context.SaveChangesAsync();

            var eventEntity = new EventBuilder()
                .WithEventTypeId(eventType.Id)
                .WithDate(DateTime.Today.AddDays(1))
                .Build();
            
            var eventEntry = await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptionsMonitor<EventArchivalJobConfig>>();
            jobConfig.CurrentValue.Returns(new EventArchivalJobConfig());

            var sut = new EventArchivalJob(_context, _jobsContext);

            //Act
            await sut.RunAsync();

            //Assert
            eventEntity.IsArchived.Should().BeFalse();
        }
    }
}
