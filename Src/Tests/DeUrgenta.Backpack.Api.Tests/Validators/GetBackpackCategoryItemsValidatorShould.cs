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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("my-weird-sub")]
        public async Task Invalidate_request_when_no_user_found_by_sub(string sub)
        {
            // Arrange
            var sut = new GetBackpackCategoryItemsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackCategoryItems(sub, Guid.NewGuid(), BackpackCategoryType.WaterAndFood));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_user_not_contributor_of_related_backpack()
        {
            // Arrange
            var sut = new GetBackpackCategoryItemsValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var contributorSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var nonContributor = new User
            {
                FirstName = "NonContributor",
                LastName = "User",
                Sub = userSub
            };

            var contributor = new User
            {
                FirstName = "Contributor",
                LastName = "User",
                Sub = contributorSub
            };

            var backpack = new Domain.Entities.Backpack
            {
                Id = backpackId,
                Name = "A backpack"
            };

            await _dbContext.Users.AddAsync(nonContributor);
            await _dbContext.Users.AddAsync(contributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = contributor });
            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackCategoryItems(nonContributor.Sub, backpackId, BackpackCategoryType.FirstAid));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_request_when_backpack_exists_and_user_contributor()
        {
            // Arrange
            string contributorSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var contributor = new User
            {
                FirstName = "Contributor",
                LastName = "User",
                Sub = contributorSub
            };

            var backpack = new Domain.Entities.Backpack
            {
                Id = backpackId,
                Name = "A backpack"
            };

            await _dbContext.Users.AddAsync(contributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = contributor });
            await _dbContext.SaveChangesAsync();

            var sut = new GetBackpackCategoryItemsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackCategoryItems(contributorSub, backpackId, BackpackCategoryType.WaterAndFood));

            // Assert
            isValid.ShouldBeTrue();
        }
        
    }
}
