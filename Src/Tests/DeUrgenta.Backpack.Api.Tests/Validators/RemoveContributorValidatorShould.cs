using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
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
            var isValid = await sut.IsValidAsync(new RemoveContributor(sub, Guid.NewGuid(), Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }


        [Fact]
        public async Task Invalidate_when_user_removes_itself()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithSub(userSub).Build();

            var backpack = new Domain.Api.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new RemoveContributor(userSub, backpack.Id, owner.Id));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();

        }

        [Fact]
        public async Task Invalidate_when_target_user_does_not_exists()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var owner = new UserBuilder().WithSub(userSub).Build();

            var backpack = new Domain.Api.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner });

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new RemoveContributor(userSub, backpack.Id, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_backpack_does_not_exist()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var contributorSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithSub(userSub).Build();
            var contributor = new UserBuilder().WithSub(contributorSub).Build();

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Users.AddAsync(contributor);

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new RemoveContributor(userSub, Guid.NewGuid(), contributor.Id));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_user_is_not_owner_of_backpack()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var contributorSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithSub(userSub).Build();
            var backpackContributor = new UserBuilder().WithSub(contributorSub).Build();

            var backpack = new Domain.Api.Entities.Backpack
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
            var isValid = await sut.IsValidAsync(new RemoveContributor(contributorSub, backpack.Id, owner.Id));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_user_is_owner_of_backpack_and_removes_a_member()
        {
            // Arrange
            var sut = new RemoveContributorValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var backpackContributorSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithSub(userSub).Build();
            var backpackContributor = new UserBuilder().WithSub(backpackContributorSub).Build();

            var backpack = new Domain.Api.Entities.Backpack
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
            var isValid = await sut.IsValidAsync(new RemoveContributor(userSub, backpack.Id, backpackContributor.Id));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}