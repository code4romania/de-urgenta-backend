using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.User.Api.CommandHandlers;
using DeUrgenta.User.Api.Commands;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.User.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class DeleteLocationHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public DeleteLocationHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<DeleteLocation>>();
            validator
                .IsValidAsync(Arg.Any<AcceptGroupInvite>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new DeleteLocationHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new DeleteLocation("a-sub", Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}