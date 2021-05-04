using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.CommandHandlers;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class CreateBackpackHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public CreateBackpackHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<CreateBackpack>>();
            validator
                .IsValidAsync(Arg.Any<CreateBackpack>())
                .Returns(Task.FromResult(false));

            var sut = new CreateBackpackHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new CreateBackpack("a-sub", new BackpackModelRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}