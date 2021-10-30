using System;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateEventValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IamI18nProvider _i18nProvider;

        public UpdateEventValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            _i18nProvider = Substitute.For<IamI18nProvider>();
            _i18nProvider.Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");
        }

        [Fact]
        public async Task Invalidate_request_when_event_does_not_exists()
        {
            // Arrange
            var sut = new UpdateEventValidator(_dbContext, _i18nProvider);

            await _dbContext.Events.AddAsync(new Event()
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
            });
            await _dbContext.SaveChangesAsync();

            // Act
            Guid eventId = Guid.NewGuid();
            var result = await sut.IsValidAsync(new UpdateEvent(eventId, new EventRequest()));

            // Assert
            result.Should().BeOfType<DetailedValidationError>();
            await _i18nProvider.Received(1).Localize(Arg.Is("event-not-exist"));
            await _i18nProvider.Received(1).Localize(Arg.Is("event-not-exist-message"), Arg.Is(eventId));
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Invalidate_request_when_invalid_event_type(int eventTypeId)
        {
            // Arrange
            var sut = new UpdateEventValidator(_dbContext, _i18nProvider);

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
            var result = await sut.IsValidAsync(new UpdateEvent(@event.Id, new EventRequest
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
            }));

            // Assert
            result.Should().BeOfType<DetailedValidationError>();
            await _i18nProvider.Received(1).Localize(Arg.Is("event-type-not-exist"));
            await _i18nProvider.Received(1).Localize(Arg.Is("event-type-not-exist-message"), Arg.Is(eventTypeId));
        }

        [Fact]
        public async Task Validate_request_when_event_exists()
        {
            // Arrange
            var sut = new UpdateEventValidator(_dbContext, _i18nProvider);

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
            var result = await sut.IsValidAsync(new UpdateEvent(@event.Id, new EventRequest
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
            }));

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}
