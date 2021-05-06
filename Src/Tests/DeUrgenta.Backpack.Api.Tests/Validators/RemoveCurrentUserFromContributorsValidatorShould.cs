using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using Shouldly;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class RemoveCurrentUserFromContributorsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public RemoveCurrentUserFromContributorsValidatorShould(DatabaseFixture fixture)
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
            var sut = new RemoveCurrentUserFromContributorsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveCurrentUserFromContributors(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_no_backpack_found()
        {
            // Arrange
            var sut = new RemoveCurrentUserFromContributorsValidator(_dbContext);

            string userSub = Guid.NewGuid().ToString();

            await _dbContext.Users.AddAsync(new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            });


            // Act
            bool isValid = await sut.IsValidAsync(new RemoveCurrentUserFromContributors(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_user_not_contributor_to_backpack()
        {
            // Arrange
            var sut = new RemoveCurrentUserFromContributorsValidator(_dbContext);
            string userSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Backpacks.AddAsync(backpack);

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveCurrentUserFromContributors(userSub, backpack.Id));

            // Assert
            isValid.ShouldBeFalse();
        }


        [Fact]
        public async Task Invalidate_when_is_owner_of_backpack()
        {
            // Arrange
            var sut = new RemoveCurrentUserFromContributorsValidator(_dbContext);
            string userSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = user, IsOwner = true});
            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveCurrentUserFromContributors(userSub, backpack.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_is_contributor()
        {
            // Arrange
            var sut = new RemoveCurrentUserFromContributorsValidator(_dbContext);
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
                FirstName = "Integration",
                LastName = "Test",
                Sub = ownerSub
            };

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };
            
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = user, IsOwner = false });

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveCurrentUserFromContributors(userSub, backpack.Id));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}