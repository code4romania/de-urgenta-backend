using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetGroupMembersValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetGroupMembersValidatorShould(DatabaseFixture fixture)
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
            var sut = new GetGroupMembersValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new GetGroupMembers(sub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_group_does_not_exists()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            var sut = new GetGroupMembersValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new GetGroupMembers(userSub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_user_not_part_of_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();
            var adminUser = new UserBuilder().Build();

            var group = new GroupBuilder().WithAdmin(adminUser).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupMembersValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new GetGroupMembers(userSub, group.Id));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_user_is_part_of_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var adminSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();
            var adminUser = new UserBuilder().WithSub(adminSub).Build();

            var group = new GroupBuilder().WithAdmin(adminUser).Build();
            
            var userToGroup = new UserToGroup {User = user, Group = group};

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupMembersValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new GetGroupMembers(userSub, group.Id));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var group = new GroupBuilder().WithAdmin(user).Build();

            var userToGroup = new UserToGroup {User = user, Group = group};

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupMembersValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new GetGroupMembers(userSub, group.Id));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}