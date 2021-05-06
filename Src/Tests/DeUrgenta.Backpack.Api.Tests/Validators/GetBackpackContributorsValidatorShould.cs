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
    public class GetBackpackContributorsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpackContributorsValidatorShould(DatabaseFixture fixture)
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
            var sut = new GetBackpackContributorsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackContributors(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }


        [Fact]
        public async Task Invalidate_when_backpack_does_not_exist()
        {
            // Arrange
            string userSub = Guid.NewGuid().ToString();
            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            var sut = new GetBackpackContributorsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackContributors(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_user_not_contributor_of_requested_backpack()
        {
            // Arrange
            string userSub = Guid.NewGuid().ToString();
            string ownerSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var owner = new User
            {
                FirstName = "Owner",
                LastName = "User",
                Sub = ownerSub
            };

            var backpack = new Domain.Entities.Backpack
            {
                Name = "A backpack"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });
            await _dbContext.SaveChangesAsync();

            var sut = new GetBackpackContributorsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackContributors(userSub, backpack.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_user_is_contributor_of_requested_backpack()
        {
            // Arrange
            string userSub = Guid.NewGuid().ToString();
            string ownerSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var owner = new User
            {
                FirstName = "Owner",
                LastName = "User",
                Sub = ownerSub
            };

            var backpack = new Domain.Entities.Backpack { Name = "A backpack" };

            await _dbContext.Users.AddAsync(user);

            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = user, IsOwner = false });

            await _dbContext.SaveChangesAsync();

            var sut = new GetBackpackContributorsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackContributors(userSub, backpack.Id));

            // Assert
            isValid.ShouldBeTrue();
        }

        [Fact]
        public async Task Validate_when_user_is_owner_of_backpack()
        {
            // Arrange
            string userSub = Guid.NewGuid().ToString();
            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var backpack = new Domain.Entities.Backpack { Name = "A backpack" };
            
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = user, IsOwner = true });

            await _dbContext.SaveChangesAsync();

            var sut = new GetBackpackContributorsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackContributors(userSub, backpack.Id));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}