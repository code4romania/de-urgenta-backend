using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.CommandHandlers;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Validators;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.CommandsHandlers
{
    [Collection("Database collection")]
    public class RemoveFromGroupHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public RemoveFromGroupHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<RemoveFromGroup>>();
            validator
                .IsValidAsync(Arg.Any<RemoveFromGroup>())
                .Returns(Task.FromResult(false));

            var sut = new RemoveFromGroupHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new RemoveFromGroup("a-sub", Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}