﻿using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Options;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AddGroupValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly GroupsConfig _groupsConfig;

        public AddGroupValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            _groupsConfig = new GroupsConfig {MaxCreatedGroupsPerUser = 5};
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
            isValid.ShouldBeFalse();
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
            isValid.ShouldBeTrue();
        }
        
        [Fact]
        public async Task Return_failed_result_when_user_exceeds_group_creation_limit()
        {
            // Arrange
            var sut = new AddGroupValidator(_dbContext, _groupsConfig);

            // Seed user
            var user = _dbContext.Users.AddAsync(new User {FirstName = "Admin", LastName = "Test", Sub = "a-sub"})
                .Result.Entity;
            await _dbContext.SaveChangesAsync();
            
            // Seed groups
            for (int i = 0; i < 5; i++)
            {
                await _dbContext.Groups.AddAsync(new Domain.Entities.Group
                {
                    Name = i.ToString(), Admin = user
                });
            }
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new AddGroup(user.Sub, new GroupRequest {Name = "TestGroup"}));

            // Assert
            result.ShouldBeFalse();
        }
    }
}