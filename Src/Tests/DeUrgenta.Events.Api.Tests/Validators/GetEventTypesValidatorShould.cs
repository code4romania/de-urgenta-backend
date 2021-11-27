using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Events.Api.Validators;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Events.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetEventTypesValidatorShould
    {
        [Fact]
        public async Task CheckGetEventTypesTest()
        {
            // Arrange
            var sut = new GetEventTypesValidator();

            // Act
            var result = await sut.IsValidAsync(new GetEventTypes());

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}
