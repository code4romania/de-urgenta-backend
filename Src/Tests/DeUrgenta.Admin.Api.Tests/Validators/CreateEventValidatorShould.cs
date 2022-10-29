using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.I18n.Service.Models;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class CreateEventValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public CreateEventValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Invalidate_request_when_invalid_event_type(int eventTypeId)
        {
            // Arrange
            var sut = new CreateEventValidator(_dbContext);

            var @event = new Event
            {
                Author = "Test",
                Title = "Test",
                ContentBody = "Test",
                PublishedOn = DateTime.UtcNow,
                Address = "A address",
                Locality = "A city",
                EventTypeId = 1,
                IsArchived = false,
                OccursOn = DateTime.Today.AddDays(30),
                OrganizedBy = "tests"
            };

            await _dbContext.Events.AddAsync(@event);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new CreateEvent(new EventRequest
            {
                Author = "Test",
                Title = "Test",
                ContentBody = "Test",
                Address = "A address",
                Locality = "A city",
                EventTypeId = eventTypeId,
                IsArchived = false,
                OccursOn = DateTime.Today.AddDays(30),
                OrganizedBy = "tests"
            }), CancellationToken.None);

            // Assert
            result
                .Should()
                .BeOfType<LocalizableValidationError>()
                .Which.Messages
                .Should()
                .BeEquivalentTo(new Dictionary<LocalizableString, LocalizableString>
                {
                    { "event-type-not-exist", new LocalizableString("event-type-not-exist-message", eventTypeId )}
                });
        }

        [Fact]
        public async Task Validate_request_when_event_type_exists()
        {
            // Arrange
            var sut = new CreateEventValidator(_dbContext);

            var @event = new Event
            {
                Author = "Test",
                Title = "Test",
                ContentBody = "Test",
                PublishedOn = DateTime.UtcNow,
                Address = "A address",
                Locality = "A city",
                EventTypeId = 1,
                IsArchived = false,
                OccursOn = DateTime.Today.AddDays(30),
                OrganizedBy = "tests"
            };

            await _dbContext.Events.AddAsync(@event);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new CreateEvent(new EventRequest
            {
                Author = "Test-new",
                Title = "Test-new",
                ContentBody = "Test-new",
                Address = "A address-new",
                Locality = "A city-new",
                EventTypeId = 2,
                IsArchived = true,
                OccursOn = DateTime.Today.AddDays(10),
                OrganizedBy = "tests-new"
            }), CancellationToken.None);

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}
