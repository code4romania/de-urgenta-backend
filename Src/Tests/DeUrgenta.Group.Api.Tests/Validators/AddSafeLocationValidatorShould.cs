using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
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
    public class AddSafeLocationValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public AddSafeLocationValidatorShould(DatabaseFixture fixture)
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
            var sut = new AddSafeLocationValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new AddSafeLocation(sub, Guid.NewGuid(), new SafeLocationRequest()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_no_group_found()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var sut = new AddSafeLocationValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new AddSafeLocation(userSub, Guid.NewGuid(), new SafeLocationRequest()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_group_does_not_exists()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            var sut = new AddSafeLocationValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new AddSafeLocation(userSub, Guid.NewGuid(), new SafeLocationRequest()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_admin_of_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var adminSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();
            var adminUser = new UserBuilder().WithSub(adminSub).Build();

            var group = new Domain.Entities.Group
            {
                Admin = adminUser,
                Name = "a group"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(adminUser);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            var sut = new AddSafeLocationValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new AddSafeLocation(userSub, Guid.NewGuid(), new SafeLocationRequest()));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_requested_group()
        {
            // Arrange
            var sut = new AddSafeLocationValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var group = new Domain.Entities.Group
            {
                Admin = user,
                Name = "my group"
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new AddSafeLocation(userSub, group.Id, new SafeLocationRequest()));

            // Assert
            isValid.Should().BeTrue();
        }
    }
}