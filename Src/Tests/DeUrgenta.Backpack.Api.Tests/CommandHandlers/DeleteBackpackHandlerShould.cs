using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.CommandHandlers;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class DeleteBackpackHandlerShould 
    {
        private readonly DeUrgentaContext _dbContext;

        public DeleteBackpackHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<DeleteBackpack>>();
            validator
                .IsValidAsync(Arg.Any<DeleteBackpack>())
                .Returns(Task.FromResult(false));

            var sut = new DeleteBackpackHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new DeleteBackpack("a-sub", Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}