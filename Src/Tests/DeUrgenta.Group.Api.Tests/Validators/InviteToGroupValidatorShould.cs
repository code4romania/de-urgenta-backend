using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Validators;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection("Database collection")]
    public class InviteToGroupValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public InviteToGroupValidatorShould(DatabaseFixture fixture)
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
            var sut = new InviteToGroupValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new InviteToGroup(sub, Guid.NewGuid(), Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_user_adds_himself_to_group()
        {
            // Arrange
            var sut = new InviteToGroupValidator(_dbContext);
            string userSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var group = new Domain.Entities.Group
            {
                Admin = user,
                Name = "my group"
            };

            var userToGroups = new UserToGroup { Group = group, User = user };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroups);
            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new InviteToGroup(userSub, group.Id, user.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_group_not_found()
        {
            // Arrange
            var sut = new InviteToGroupValidator(_dbContext);
            string userSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new InviteToGroup(userSub, Guid.NewGuid(), Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_invited_user_not_found()
        {
            // Arrange
            var sut = new InviteToGroupValidator(_dbContext);
            string userSub = Guid.NewGuid().ToString();

            var admin = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var group = new Domain.Entities.Group
            {
                Admin = admin,
                Name = "my group"
            };

            var userToGroups = new UserToGroup { Group = group, User = admin };

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroups);

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new InviteToGroup(userSub, group.Id, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_user_is_already_invited()
        {
            // Arrange
            var sut = new InviteToGroupValidator(_dbContext);

            string userSub = Guid.NewGuid().ToString();
            string nonGroupUserSub = Guid.NewGuid().ToString();

            var admin = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var invitedGroupUser = new User
            {
                FirstName = "Integration2",
                LastName = "Test2",
                Sub = nonGroupUserSub
            };

            var group = new Domain.Entities.Group
            {
                Admin = admin,
                Name = "my group"
            };

            var userToGroups = new UserToGroup { Group = group, User = admin };
            var groupInvite = new GroupInvite
            {
                Group = group,
                InvitationReceiver = invitedGroupUser,
                InvitationSender = admin
            };

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Users.AddAsync(invitedGroupUser);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroups);
            await _dbContext.GroupInvites.AddAsync(groupInvite);

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new InviteToGroup(userSub, group.Id, invitedGroupUser.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_group_and_invited_existing_user()
        {
            // Arrange
            var sut = new InviteToGroupValidator(_dbContext);
           
            string userSub = Guid.NewGuid().ToString();
            string nonGroupUserSub = Guid.NewGuid().ToString();

            var admin = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var nonGroupUser = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = nonGroupUserSub
            };

            var group = new Domain.Entities.Group
            {
                Admin = admin,
                Name = "my group"
            };

            var userToGroups = new UserToGroup { Group = group, User = admin };

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Users.AddAsync(nonGroupUser);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroups);

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new InviteToGroup(userSub, group.Id, nonGroupUser.Id));

            // Assert
            isValid.ShouldBeTrue();
        }

        [Fact]
        public async Task Validate_when_user_is_part_of_group_and_invited_existing_user()
        {
            // Arrange
            var sut = new InviteToGroupValidator(_dbContext);
            string userSub = Guid.NewGuid().ToString();
            string nonGroupUserSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var nonGroupUser = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = nonGroupUserSub
            };

            var group = new Domain.Entities.Group
            {
                Admin = user,
                Name = "my group"
            };

            var userToGroups = new UserToGroup { Group = group, User = user };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(nonGroupUser);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroups);

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new InviteToGroup(userSub, group.Id, nonGroupUser.Id));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}