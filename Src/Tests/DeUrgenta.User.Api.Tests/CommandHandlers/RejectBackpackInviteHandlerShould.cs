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
    public class RejectBackpackInviteHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public RejectBackpackInviteHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<RejectBackpackInvite>>();
            validator
                .IsValidAsync(Arg.Any<RejectBackpackInvite>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new RejectBackpackInviteHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new RejectBackpackInvite("a-sub", Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}