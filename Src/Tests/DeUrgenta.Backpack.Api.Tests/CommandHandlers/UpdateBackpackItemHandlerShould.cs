using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.CommandHandlers;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.CommandHandlers
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
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new UpdateBackpackItemHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new UpdateBackpackItem("a-sub", Guid.NewGuid(), new Models.BackpackItemRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
