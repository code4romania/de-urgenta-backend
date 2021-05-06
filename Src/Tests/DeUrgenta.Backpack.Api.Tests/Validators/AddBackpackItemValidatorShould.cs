using System;
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
    public class AddBackpackItemValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        public AddBackpackItemValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Invalidate_request_when_backpack_does_not_exist()
        {
            // Arrange
            var sut = new AddBackpackItemValidator(_dbContext);
            var command = new AddBackpackItem(Guid.NewGuid(), new BackpackItemRequest());

            // Act
            bool isValid = await sut.IsValidAsync(command);

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_request_when_backpack_exists()
        {
            // Arrange
            var sut = new AddBackpackItemValidator(_dbContext);

            var userId = Guid.NewGuid();
            var backpackId = Guid.NewGuid();
            await _dbContext.Users.AddAsync(new User
            {
                Id = userId,
                FirstName = "test-user",
                LastName = "test-user"
            });
            await _dbContext.Backpacks.AddAsync(new Domain.Entities.Backpack
            {
                Id = backpackId,
                Name = "test-backpack",
                AdminUserId = userId
            });

            await _dbContext.SaveChangesAsync();

            var command = new AddBackpackItem(backpackId, new BackpackItemRequest());

            // Act
            bool isValid = await sut.IsValidAsync(command);

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}
