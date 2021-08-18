using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Validators;
using Shouldly;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AcceptBackpackInviteValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public AcceptBackpackInviteValidatorShould(DatabaseFixture fixture)
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
            var sut = new AcceptBackpackInviteValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new AcceptBackpackInvite(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_invite_is_for_other_user()
        {
            var sut = new AcceptBackpackInviteValidator(_dbContext);

            // Arrange
            string userSub = Guid.NewGuid().ToString();
            var user = new DeUrgenta.Domain.Entities.User
            {
                FirstName = "user",
                LastName = "user",
                Sub = userSub
            };

            string otherUserSub = Guid.NewGuid().ToString();
            var otherUser = new DeUrgenta.Domain.Entities.User
            {
                FirstName = "other",
                LastName = "user",
                Sub = otherUserSub
            };

            string adminSub = Guid.NewGuid().ToString();
            var admin = new DeUrgenta.Domain.Entities.User
            {
                FirstName = "Admin",
                LastName = "Test",
                Sub = adminSub
            };

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
            bool isValid = await sut.IsValidAsync(new AcceptBackpackInvite(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_invite_does_not_exist()
        {
            var sut = new AcceptBackpackInviteValidator(_dbContext);

            // Arrange
            string userSub = Guid.NewGuid().ToString();
            var user = new DeUrgenta.Domain.Entities.User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            string adminSub = Guid.NewGuid().ToString();
            var admin = new DeUrgenta.Domain.Entities.User
            {
                FirstName = "Admin",
                LastName = "Test",
                Sub = adminSub
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(admin);

            var backpack = new Backpack
            {
                Name = "backpack"
            };

            await _dbContext.Backpacks.AddAsync(backpack);

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new AcceptBackpackInvite(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_an_invite_exists()
        {
            var sut = new AcceptBackpackInviteValidator(_dbContext);

            // Arrange
            string userSub = Guid.NewGuid().ToString();
            var user = new DeUrgenta.Domain.Entities.User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            string adminSub = Guid.NewGuid().ToString();
            var admin = new DeUrgenta.Domain.Entities.User
            {
                FirstName = "Admin",
                LastName = "Test",
                Sub = adminSub
            };

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
            bool isValid = await sut.IsValidAsync(new AcceptBackpackInvite(userSub, backpackInvite.Id));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}