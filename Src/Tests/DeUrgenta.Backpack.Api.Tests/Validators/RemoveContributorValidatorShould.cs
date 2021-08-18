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
    public class RemoveContributorValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public RemoveContributorValidatorShould(DatabaseFixture fixture)
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
            var sut = new RemoveContributorValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveContributor(sub, Guid.NewGuid(), Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }


        [Fact]
        public async Task Invalidate_when_user_removes_itself()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            string userSub = Guid.NewGuid().ToString();

            var owner = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var backpack = new Domain.Entities.Backpack()
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser() { Backpack = backpack, User = owner ,IsOwner = true});

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveContributor(userSub, backpack.Id, owner.Id));

            // Assert
            isValid.ShouldBeFalse();

        }

        [Fact]
        public async Task Invalidate_when_target_user_does_not_exists()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            string userSub = Guid.NewGuid().ToString();

            var owner = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var backpack = new Domain.Entities.Backpack()
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser() { Backpack = backpack, User = owner });

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveContributor(userSub, backpack.Id, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_backpack_does_not_exist()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            string userSub = Guid.NewGuid().ToString();
            string contributorUserSub = Guid.NewGuid().ToString();

            var owner = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var contributor = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = contributorUserSub
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Users.AddAsync(contributor);

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveContributor(userSub, Guid.NewGuid(), contributor.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_user_is_not_owner_of_backpack()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            string userSub = Guid.NewGuid().ToString();
            string contributorUserSub = Guid.NewGuid().ToString();

            var owner = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var backpackContributor = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = contributorUserSub
            };

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Users.AddAsync(backpackContributor);

            await _dbContext.Backpacks.AddAsync(backpack);

            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = backpackContributor, IsOwner = false });

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveContributor(contributorUserSub, backpack.Id, owner.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_user_is_owner_of_backpack_and_removes_a_member()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            string userSub = Guid.NewGuid().ToString();
            string backpackContributorSub = Guid.NewGuid().ToString();

            var owner = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var backpackContributor = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = backpackContributorSub
            };

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Users.AddAsync(backpackContributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = backpackContributor, IsOwner = false });

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new RemoveContributor(userSub, backpack.Id, backpackContributor.Id));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}