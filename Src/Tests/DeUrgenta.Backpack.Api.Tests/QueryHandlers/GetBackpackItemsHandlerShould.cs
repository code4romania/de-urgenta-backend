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
    public class GetBackpackItemsHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpackItemsHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetBackpackItems>>();
            validator
                .IsValidAsync(Arg.Any<GetBackpackItems>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new GetBackpackItemsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetBackpackItems("a-sub", Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
