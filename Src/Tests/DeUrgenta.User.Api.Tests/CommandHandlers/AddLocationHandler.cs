using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
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
    public class AddLocationHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public AddLocationHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<AddLocation>>();
            validator
                .IsValidAsync(Arg.Any<AddLocation>(), CancellationToken.None)
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            UserLocationRequest userLocationRequest = new();

            var sut = new AddLocationHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new AddLocation("a-sub", userLocationRequest), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}