using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateGroupValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateGroupValidatorShould(DatabaseFixture fixture)
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
            var sut = new UpdateGroupValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new UpdateGroup(sub, Guid.NewGuid(), new GroupRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_no_group_found()
        {
            // Arrange
            var sut = new UpdateGroupValidator(_dbContext);
            var userSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new UpdateGroup(userSub, Guid.NewGuid(), new GroupRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_admin()
        {
            // Arrange
            var sut = new UpdateGroupValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var groupUserSub = Guid.NewGuid().ToString();

            var admin = new UserBuilder().WithSub(userSub).Build();
            var groupUser = new UserBuilder().WithSub(groupUserSub).Build();

            var group = new GroupBuilder().WithAdmin(admin).Build();

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Users.AddAsync(groupUser);

            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup {Group = group, User = admin});
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup {Group = group, User = groupUser});

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new UpdateGroup(groupUserSub, Guid.NewGuid(), new GroupRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_group()
        {
            // Arrange
            var sut = new UpdateGroupValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();

            var admin = new UserBuilder().WithSub(userSub).Build();

            var group = new GroupBuilder().WithAdmin(admin).Build();

            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup {Group = group, User = admin});

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new UpdateGroup(userSub, group.Id, new GroupRequest()));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}