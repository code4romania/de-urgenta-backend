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
    public class RemoveContributorHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public RemoveContributorHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<RemoveContributor>>();
            validator
                .IsValidAsync(Arg.Any<RemoveContributor>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new RemoveContributorHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new RemoveContributor("a-sub", Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}