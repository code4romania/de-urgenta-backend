using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.QueryHandlers;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using Shouldly;
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
                .Returns(Task.FromResult(false));

            var sut = new GetBackpackItemsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetBackpackItems(Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }

        [Fact]
        public async Task Return_success_result_when_validation_succeeds()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetBackpackItems>>();
            validator
                .IsValidAsync(Arg.Any<GetBackpackItems>())
                .Returns(Task.FromResult(true));
            var backpack  = new Domain.Entities.Backpack { Id = Guid.NewGuid()};
            var backpackItemId = Guid.NewGuid();
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpackItem.AddAsync(new BackpackItem { Id = Guid.NewGuid(), Backpack = backpack });
            var sut = new GetBackpackItemsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetBackpackItems(backpackItemId), CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
        }
    }
}
