using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using Shouldly;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateBackpackItemValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        public UpdateBackpackItemValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Invalidate_request_when_backpack_item_does_not_exist()
        {
            // Arrange
            var sut = new UpdateBackpackItemValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateBackpackItem(Guid.NewGuid(), new BackpackItemRequest()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_request_when_backpack_item_exists()
        {
            // Arrange
            var backpackItemId = Guid.NewGuid();

            var backpack = new Domain.Entities.Backpack
            {
                Id = Guid.NewGuid(),
                Name = "test-backpack"
            };

            await _dbContext.Backpacks.AddAsync(backpack);

            await _dbContext.BackpackItem.AddAsync(new BackpackItem
            {
                Id = backpackItemId,
                Name = "test-backpack-item",
                Backpack = backpack
            });

            await _dbContext.SaveChangesAsync();

            var sut = new UpdateBackpackItemValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateBackpackItem(backpackItemId, new BackpackItemRequest()));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}
