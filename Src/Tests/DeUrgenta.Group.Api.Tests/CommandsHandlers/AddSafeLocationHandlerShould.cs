using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.CommandHandlers;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.CommandsHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
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
                .IsValidAsync(Arg.Any<AddSafeLocation>(), CancellationToken.None)
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new AddSafeLocationHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new AddSafeLocation("a-sub", Guid.NewGuid(), new SafeLocationRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}