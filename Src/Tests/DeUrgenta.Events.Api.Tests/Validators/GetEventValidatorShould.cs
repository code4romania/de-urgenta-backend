using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Events.Api.Validators;
using DeUrgenta.Tests.Helpers;
using Shouldly;
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
        [InlineData(null, null)]
        [InlineData(null, 1)]
        [InlineData("some city", null)]
        public async Task CheckGetEventCitiesTest(string city, int? eventTypeId)
        {
            // Arrange
            var sut = new GetEventValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetEvent(new Models.EventModelRequest { City = city, EventTypeId = eventTypeId }));

            // Assert
            isValid.ShouldBeFalse();
        }
    }
}
