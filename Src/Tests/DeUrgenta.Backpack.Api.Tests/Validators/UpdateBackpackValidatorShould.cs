using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateBackpackValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateBackpackValidatorShould(DatabaseFixture fixture)
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
            var sut = new UpdateBackpackValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new UpdateBackpack(sub, Guid.NewGuid(), new BackpackModelRequest()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_no_backpack_found()
        {
            // Arrange
            var sut = new UpdateBackpackValidator(_dbContext);
            var userSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new UpdateBackpack(userSub, Guid.NewGuid(), new BackpackModelRequest()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_owner()
        {
            // Arrange
            var sut = new UpdateBackpackValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var backpackContributorSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithSub(userSub).Build();
            var backpackContributor = new UserBuilder().WithSub(backpackContributorSub).Build();

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
            var isValid = await sut.IsValidAsync(new UpdateBackpack(backpackContributorSub, Guid.NewGuid(), new BackpackModelRequest()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Validate_when_user_is_owner_of_backpack()
        {
            // Arrange
            var sut = new UpdateBackpackValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithSub(userSub).Build();

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(owner);

            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });
            
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new UpdateBackpack(userSub, backpack.Id, new BackpackModelRequest()));

            // Assert
            isValid.Should().BeTrue();
        }
    }
}