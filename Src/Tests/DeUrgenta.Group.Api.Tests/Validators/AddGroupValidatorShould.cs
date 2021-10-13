using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Options;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AddGroupValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IOptions<GroupsConfig> _groupsConfig;

        public AddGroupValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            var options = new GroupsConfig {MaxCreatedGroupsPerUser = 5};
            _groupsConfig = Microsoft.Extensions.Options.Options.Create(options);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("my-weird-sub")]
        public async Task Invalidate_request_when_no_user_found_by_sub(string sub)
        {
            // Arrange
            var sut = new AddGroupValidator(_dbContext, _groupsConfig);

            // Act
            var isValid = await sut.IsValidAsync(new AddGroup(sub, new GroupRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_user_was_found_by_sub()
        {
            var sut = new AddGroupValidator(_dbContext, _groupsConfig);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new AddGroup(userSub, new GroupRequest()));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }

        [Fact]
        public async Task Invalidate_when_user_exceeds_group_creation_limit()
        {
            // Arrange
            var sut = new AddGroupValidator(_dbContext, _groupsConfig);

            // Seed user
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();
            await _dbContext.SaveChangesAsync();

            // Seed groups
            for (int i = 0; i < 5; i++)
            {
                await _dbContext.Groups.AddAsync(new GroupBuilder().WithAdmin(user).Build());
            }

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new AddGroup(user.Sub, new GroupRequest {Name = "TestGroup"}));

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }
    }
}