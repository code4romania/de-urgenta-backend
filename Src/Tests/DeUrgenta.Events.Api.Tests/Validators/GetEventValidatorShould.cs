using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Events.Api.Validators;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Events.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetEventValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IamI18nProvider _i18nProvider;

        public GetEventValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            _i18nProvider = Substitute.For<IamI18nProvider>();
            _i18nProvider.Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        public async Task ShouldInvalidateWhenInvalidEventTypeId(int? eventTypeId)
        {
            // Arrange
            var sut = new GetEventValidator(_dbContext, _i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new GetEvent(new Models.EventModelRequest { EventTypeId = eventTypeId }));

            // Assert
            isValid.Should().BeOfType<DetailedValidationError>();
            await _i18nProvider.Received(1).Localize(Arg.Is("event-type-not-exist"));
            await _i18nProvider.Received(1).Localize(Arg.Is("event-type-not-exist-message"), Arg.Is(eventTypeId));
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldValidateWhenValidEventTypeId(int? eventTypeId)
        {
            // Arrange
            var sut = new GetEventValidator(_dbContext, _i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new GetEvent(new Models.EventModelRequest { EventTypeId = eventTypeId }));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}
