using System;
using System.Collections.Generic;
using System.Threading;
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
    public class DeleteGroupValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public DeleteGroupValidatorShould(DatabaseFixture fixture)
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
            var sut = new DeleteGroupValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new DeleteGroup(sub, Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_group_does_not_exists()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var sut = new DeleteGroupValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new DeleteGroup(userSub, Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_part_of_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var adminSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();
            var adminUser = new UserBuilder().WithSub(adminSub).Build();

            var group = new GroupBuilder().WithAdmin(adminUser).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(adminUser);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            var sut = new DeleteGroupValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new DeleteGroup(userSub, group.Id), CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }


        [Fact]
        public async Task Invalidate_request_when_user_is_not_admin_of_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var adminSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();
            var adminUser = new UserBuilder().WithSub(adminSub).Build();

            var group = new GroupBuilder().WithAdmin(adminUser).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(adminUser);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup
            {
                Group = group,
                User = user
            });

            await _dbContext.SaveChangesAsync();

            var sut = new DeleteGroupValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new DeleteGroup(userSub, group.Id), CancellationToken.None);

            // Assert
            result
                .Should()
                .BeOfType<LocalizableValidationError>()
                .Which.Messages
                .Should()
                .BeEquivalentTo(new Dictionary<LocalizableString, LocalizableString>
                {
                    { "cannot-delete-group", "only-group-admin-can-delete-group-message" }
                });
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_requested_group()
        {
            // Arrange
            var sut = new DeleteGroupValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var group = new GroupBuilder().WithAdmin(user).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup
            {
                Group = group,
                User = user
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new DeleteGroup(userSub, group.Id), CancellationToken.None);

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}