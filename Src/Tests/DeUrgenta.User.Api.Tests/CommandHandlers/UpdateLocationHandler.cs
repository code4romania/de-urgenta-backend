using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.User.Api.CommandHandlers;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Models;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.User.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateLocationHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateLocationHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<UpdateLocation>>();
            validator
                .IsValidAsync(Arg.Any<UpdateLocation>())
                .Returns(Task.FromResult(false));
            UserLocationRequest userLocationRequest = new();

            var sut = new UpdateLocationHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new UpdateLocation("a-sub", Guid.NewGuid(), userLocationRequest), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}