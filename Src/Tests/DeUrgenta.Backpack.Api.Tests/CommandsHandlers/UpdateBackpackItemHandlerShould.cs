using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.CommandsHandlers;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.CommandsHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateBackpackItemHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateBackpackItemHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<UpdateBackpackItem>>();
            validator
                .IsValidAsync(Arg.Any<UpdateBackpackItem>())
                .Returns(Task.FromResult(false));

            var sut = new UpdateBackpackItemHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new UpdateBackpackItem(Guid.NewGuid(), new Models.BackpackItemRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}
