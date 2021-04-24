using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.CommandHandlers;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Validators;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.CommandsHandlers
{
    [Collection("Database collection")]
    public class UpdateSafeLocationHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateSafeLocationHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<UpdateSafeLocation>>();
            validator
                .IsValidAsync(Arg.Any<UpdateSafeLocation>())
                .Returns(Task.FromResult(false));

            var sut = new UpdateSafeLocationHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new UpdateSafeLocation("a-sub", Guid.NewGuid(), Guid.NewGuid(), new SafeLocationRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}