using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetGroupSafeLocationsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetGroupSafeLocationsValidatorShould(DatabaseFixture fixture)
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
            var sut = new GetGroupSafeLocationsValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new GetGroupSafeLocations(sub, Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_no_group_found()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupSafeLocationsValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new GetGroupSafeLocations(userSub, Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_not_part_of_the_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var adminSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();
            var admin = new UserBuilder().WithSub(adminSub).Build();

            var group = new GroupBuilder().WithAdmin(admin).Build();

            var adminToGroup = new UserToGroup {Group = group, User = admin};

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(adminToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupSafeLocationsValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new GetGroupSafeLocations(userSub, group.Id), CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_request_when_is_admin_of_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();

            var group = new GroupBuilder().WithAdmin(user).Build();
            var userToGroup = new UserToGroup {Group = group, User = user};

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupSafeLocationsValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new GetGroupSafeLocations(userSub, group.Id), CancellationToken.None);

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }

        [Fact]
        public async Task Validate_request_when_is_part_of_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var adminSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();
            var admin = new UserBuilder().WithSub(adminSub).Build();

            var group = new GroupBuilder().WithAdmin(admin).Build();

            var userToGroup = new UserToGroup {Group = group, User = user};
            var adminToGroup = new UserToGroup {Group = group, User = admin};

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroup);
            await _dbContext.UsersToGroups.AddAsync(adminToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupSafeLocationsValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new GetGroupSafeLocations(userSub, group.Id), CancellationToken.None);

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}