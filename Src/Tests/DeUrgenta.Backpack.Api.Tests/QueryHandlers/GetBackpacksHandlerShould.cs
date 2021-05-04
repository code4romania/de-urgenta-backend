using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.QueryHandlers;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.QueryHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetBackpacksHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpacksHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetBackpacks>>();
            validator
                .IsValidAsync(Arg.Any<GetBackpacks>())
                .Returns(Task.FromResult(false));

            var sut = new GetBackpacksHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetBackpacks("a-sub"), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}