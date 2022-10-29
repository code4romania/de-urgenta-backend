using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("my-weird-sub")]
        public async Task Invalidate_request_when_no_user_found_by_sub(string sub)
        {
            // Arrange
            var sut = new UpdateBackpackItemValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new UpdateBackpackItem(sub, Guid.NewGuid(), new BackpackItemRequest()),
                CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_user_not_contributor_of_related_backpack()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var contributorSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var nonContributor = new UserBuilder().WithSub(userSub).Build();
            var contributor = new UserBuilder().WithSub(contributorSub).Build();

            var backpack = new Domain.Api.Entities.Backpack
            {
                Id = backpackId,
                Name = "A backpack"
            };

            await _dbContext.Users.AddAsync(nonContributor);
            await _dbContext.Users.AddAsync(contributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = contributor });
            await _dbContext.SaveChangesAsync();

            var sut = new UpdateBackpackItemValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new UpdateBackpackItem(nonContributor.Sub, backpackId, new BackpackItemRequest()), CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_item_does_not_exist()
        {
            // Arrange
            var contributorSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var contributor = new UserBuilder().WithSub(contributorSub).Build();

            var backpack = new Domain.Api.Entities.Backpack
            {
                Id = backpackId,
                Name = "A backpack"
            };

            await _dbContext.Users.AddAsync(contributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = contributor });
            await _dbContext.SaveChangesAsync();

            var sut = new UpdateBackpackItemValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new UpdateBackpackItem(contributorSub, Guid.NewGuid(), new BackpackItemRequest()),
                CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }
       

        [Fact]
        public async Task Validate_request_when_backpack_item_exists_and_user_contributor()
        {
            // Arrange
            var contributorSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();
            var backpackItemId = Guid.NewGuid();

            var contributor = new UserBuilder().WithSub(contributorSub).Build();

            var backpack = new Domain.Api.Entities.Backpack
            {
                Id = backpackId,
                Name = "A backpack"
            };

            await _dbContext.BackpackItems.AddAsync(new BackpackItem
            {
                Id = backpackItemId,
                Name = "test-backpack-item",
                Backpack = backpack
            });

            await _dbContext.Users.AddAsync(contributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = contributor });
            await _dbContext.SaveChangesAsync();

            var sut = new UpdateBackpackItemValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new UpdateBackpackItem(contributorSub, backpackItemId, new BackpackItemRequest()),
                CancellationToken.None);

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}
