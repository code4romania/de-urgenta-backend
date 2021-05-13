using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using Shouldly;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetBackpackCategoryItemsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpackCategoryItemsValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Invalidate_request_when_backpack_does_not_exist()
        {
            // Arrange
            var sut = new GetBackpackCategoryItemsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackCategoryItems(Guid.NewGuid(), BackpackCategoryType.WaterAndFood));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_request_when_backpack_exists()
        {
            // Arrange
            var backpackId = Guid.NewGuid();
           
            await _dbContext.Backpacks.AddAsync(new Domain.Entities.Backpack
            {
                Id = backpackId,
                Name = "test-backpack"
            });
            await _dbContext.SaveChangesAsync();

            var sut = new GetBackpackCategoryItemsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackCategoryItems(backpackId, BackpackCategoryType.WaterAndFood));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}
