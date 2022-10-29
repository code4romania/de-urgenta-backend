using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.CommandHandlers;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AddBackpackItemHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public AddBackpackItemHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<AddBackpackItem>>();
            validator
                .IsValidAsync(Arg.Any<AddBackpackItem>(), CancellationToken.None)
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new AddBackpackItemHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new AddBackpackItem("a-sub", Guid.NewGuid(), new BackpackItemRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
