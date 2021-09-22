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
    public class GetEventTypesValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetEventTypesValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task CheckGetEventTypesTest()
        {
            // Arrange
            var sut = new GetEventTypesValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetEventTypes());

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}
