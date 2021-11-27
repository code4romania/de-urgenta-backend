using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.I18n.Service.Models;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class RemoveFromGroupValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public RemoveFromGroupValidatorShould(DatabaseFixture fixture)
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
            var sut = new RemoveFromGroupValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new RemoveFromGroup(sub, Guid.NewGuid(), Guid.NewGuid()));

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_user_removes_itself()
        {
            // Arrange
            var sut = new RemoveFromGroupValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();

            var admin = new UserBuilder().WithSub(userSub).Build();

            var group = new GroupBuilder().WithAdmin(admin).Build();

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup { Group = group, User = admin });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new RemoveFromGroup(userSub, group.Id, admin.Id));

            // Assert
            result
                .Should()
                .BeOfType<LocalizableValidationError>()
                .Which.Messages
                .Should()
                .BeEquivalentTo(new Dictionary<LocalizableString, LocalizableString>
                {
                    { "cannot-remove-user","cannot-remove-yourself-message" }
                });
        }

        [Fact]
        public async Task Invalidate_when_target_user_does_not_exists()
        {
            // Arrange
            var sut = new RemoveFromGroupValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();

            var admin = new UserBuilder().WithSub(userSub).Build();

            var group = new GroupBuilder().WithAdmin(admin).Build();

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup { Group = group, User = admin });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new RemoveFromGroup(userSub, group.Id, Guid.NewGuid()));

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_group_does_not_exist()
        {
            // Arrange
            var sut = new RemoveFromGroupValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var groupUserSub = Guid.NewGuid().ToString();

            var admin = new UserBuilder().WithSub(userSub).Build();
            var groupUser = new UserBuilder().WithSub(groupUserSub).Build();

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Users.AddAsync(groupUser);

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new RemoveFromGroup(userSub, Guid.NewGuid(), groupUser.Id));

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_user_is_not_admin_of_group()
        {
            // Arrange
            var sut = new RemoveFromGroupValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var groupUserSub = Guid.NewGuid().ToString();

            var admin = new UserBuilder().WithSub(userSub).Build();
            var groupUser = new UserBuilder().WithSub(groupUserSub).Build();
            var nonGroupUser = new UserBuilder().Build();

            var group = new GroupBuilder().WithAdmin(admin).Build();

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Users.AddAsync(groupUser);
            await _dbContext.Users.AddAsync(nonGroupUser);

            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup { Group = group, User = admin });
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup { Group = group, User = groupUser });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new RemoveFromGroup(groupUserSub, group.Id, nonGroupUser.Id));

            // Assert
            result
                .Should()
                .BeOfType<LocalizableValidationError>()
                .Which.Messages
                .Should()
                .BeEquivalentTo(new Dictionary<LocalizableString, LocalizableString>
                {
                    { "cannot-remove-user","only-group-admin-can-remove-users-message" }
                });
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_a_group_and_removes_a_member()
        {
            // Arrange
            var sut = new RemoveFromGroupValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var groupUserSub = Guid.NewGuid().ToString();

            var admin = new UserBuilder().WithSub(userSub).Build();
            var groupUser = new UserBuilder().WithSub(groupUserSub).Build();

            var group = new GroupBuilder().WithAdmin(admin).Build();

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Users.AddAsync(groupUser);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup { Group = group, User = admin });
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup { Group = group, User = groupUser });

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new RemoveFromGroup(userSub, group.Id, groupUser.Id));

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}