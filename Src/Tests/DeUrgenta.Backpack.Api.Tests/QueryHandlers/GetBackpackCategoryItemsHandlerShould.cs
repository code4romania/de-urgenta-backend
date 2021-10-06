using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.QueryHandlers;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.QueryHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetBackpackCategoryItemsHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpackCategoryItemsHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetBackpackCategoryItems>>();
            validator
                .IsValidAsync(Arg.Any<GetBackpackCategoryItems>())
                .Returns(Task.FromResult(false));

            var sut = new GetBackpackCategoryItemsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetBackpackCategoryItems("a-sub", Guid.NewGuid(), BackpackCategoryType.FirstAid), CancellationToken.None);
             
            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
