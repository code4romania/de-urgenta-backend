using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Events.Api.Validators;
using DeUrgenta.I18n.Service.Models;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Events.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetEventValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetEventValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        public async Task ShouldInvalidateWhenInvalidEventTypeId(int? eventTypeId)
        {
            // Arrange
            var sut = new GetEventValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new GetEvent(new Models.EventModelRequest { EventTypeId = eventTypeId }),
                CancellationToken.None);

            // Assert
            result
                .Should()
                .BeOfType<LocalizableValidationError>()
                .Which.Messages
                .Should()
                .BeEquivalentTo(new Dictionary<LocalizableString, LocalizableString>
                {
                    { "event-type-not-exist",new LocalizableString("event-type-not-exist-message", eventTypeId) }
                });
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldValidateWhenValidEventTypeId(int? eventTypeId)
        {
            // Arrange
            var sut = new GetEventValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new GetEvent(new Models.EventModelRequest { EventTypeId = eventTypeId }),
                CancellationToken.None);

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}
