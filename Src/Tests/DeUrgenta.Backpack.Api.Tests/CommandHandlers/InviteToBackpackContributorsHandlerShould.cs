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
    public class InviteToBackpackContributorsHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public InviteToBackpackContributorsHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<InviteToBackpackContributors>>();
            validator
                .IsValidAsync(Arg.Any<InviteToBackpackContributors>())
                .Returns(Task.FromResult(false));

            var sut = new InviteToBackpackContributorsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new InviteToBackpackContributors("a-sub", Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}