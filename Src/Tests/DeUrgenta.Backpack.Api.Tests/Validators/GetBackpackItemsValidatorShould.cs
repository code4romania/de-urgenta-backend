using System;
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
    public class GetBackpackItemsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        public GetBackpackItemsValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }
        [Fact]
        public async Task Invalidate_request_when_backpack_does_not_exist()
        {
            // Arrange
            var sut = new GetBackpackItemsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackItems(Guid.NewGuid()));

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

            var sut = new GetBackpackItemsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackItems(backpackId));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}
