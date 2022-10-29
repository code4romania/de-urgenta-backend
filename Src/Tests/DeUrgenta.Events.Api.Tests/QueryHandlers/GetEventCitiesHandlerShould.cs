using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Events.Api.QueryHandlers;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Events.Api.Tests.QueryHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetEventCitiesHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetEventCitiesHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetEventCities>>();
            validator
                .IsValidAsync(Arg.Any<GetEventCities>(), CancellationToken.None)
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new GetEventCitiesHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetEventCities(null), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
