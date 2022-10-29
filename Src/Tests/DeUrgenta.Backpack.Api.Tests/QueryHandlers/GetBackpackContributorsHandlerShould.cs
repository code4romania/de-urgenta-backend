using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.QueryHandlers;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.QueryHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetBackpackContributorsHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpackContributorsHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetBackpackContributors>>();
            validator
                .IsValidAsync(Arg.Any<GetBackpackContributors>(), CancellationToken.None)
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new GetBackpackContributorsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetBackpackContributors("a-sub", Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}