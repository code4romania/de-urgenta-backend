using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
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
    public class AddSafeLocationHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public AddSafeLocationHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<AddSafeLocation>>();
            validator
                .IsValidAsync(Arg.Any<AddSafeLocation>())
                .Returns(Task.FromResult(false));

            var sut = new AddSafeLocationHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new AddSafeLocation("a-sub", Guid.NewGuid(), new SafeLocationRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}