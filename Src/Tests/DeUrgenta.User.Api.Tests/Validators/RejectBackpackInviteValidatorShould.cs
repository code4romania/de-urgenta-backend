using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Validators;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class RejectBackpackInviteValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public RejectBackpackInviteValidatorShould(DatabaseFixture fixture)
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
            var sut = new RejectBackpackInviteValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new RejectBackpackInvite(sub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_when_invite_is_for_other_user()
        {
            var sut = new RejectBackpackInviteValidator(_dbContext);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var otherUserSub = Guid.NewGuid().ToString();
            var otherUser = new UserBuilder().WithSub(otherUserSub).Build();

            var adminSub = Guid.NewGuid().ToString();
            var admin = new UserBuilder().WithSub(adminSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Users.AddAsync(otherUser);

            var backpack = new Backpack
            {
                Name = "backpack"
            };

            await _dbContext.Backpacks.AddAsync(backpack);

            var backpackInvite = new BackpackInvite
            {
                InvitationReceiver = user,
                InvitationSender = admin,
                Backpack = backpack
            };

            await _dbContext.BackpackInvites.AddAsync(backpackInvite);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new RejectBackpackInvite(userSub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_when_invite_does_not_exist()
        {
            var sut = new RejectBackpackInviteValidator(_dbContext);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var adminSub = Guid.NewGuid().ToString();
            var admin = new UserBuilder().WithSub(adminSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(admin);

            var backpack = new Backpack
            {
                Name = "backpack"
            };

            await _dbContext.Backpacks.AddAsync(backpack);

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new RejectBackpackInvite(userSub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Validate_when_an_invite_exists()
        {
            var sut = new RejectBackpackInviteValidator(_dbContext);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var adminSub = Guid.NewGuid().ToString();
            var admin = new UserBuilder().WithSub(adminSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(admin);

            var backpack = new Backpack
            {
                Name = "backpack"
            };

            var backpackInvite = new BackpackInvite
            {
                InvitationReceiver = user,
                InvitationSender = admin,
                Backpack = backpack
            };
            await _dbContext.BackpackInvites.AddAsync(backpackInvite);
            await _dbContext.Backpacks.AddAsync(backpack);

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new RejectBackpackInvite(userSub, backpackInvite.Id));

            // Assert
            isValid.Should().BeTrue();
        }
    }
}