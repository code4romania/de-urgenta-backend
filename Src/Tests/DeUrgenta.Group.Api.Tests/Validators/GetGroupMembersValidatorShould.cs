using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Validators;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection("Database collection")]
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
            bool isValid = await sut.IsValidAsync(new GetGroupMembers(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_group_does_not_exists()
        {
            // Arrange
            string userSub = Guid.NewGuid().ToString();
            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            var sut = new GetGroupMembersValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetGroupMembers(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_user_not_part_of_group()
        {
            // Arrange
            string userSub = Guid.NewGuid().ToString();
            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var adminUser = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Sub = "admin sub"
            };

            var group = new Domain.Entities.Group { Admin = adminUser, Name = "A group" };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupMembersValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetGroupMembers(userSub, group.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_user_is_part_of_group()
        {
            // Arrange
            string userSub = Guid.NewGuid().ToString();
            string adminSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var adminUser = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Sub = adminSub
            };

            var group = new Domain.Entities.Group { Admin = adminUser, Name = "A group" };

            var userToGroup = new UserToGroup { User = user, Group = group};

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupMembersValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetGroupMembers(userSub, group.Id));

            // Assert
            isValid.ShouldBeTrue();
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_group()
        {
            // Arrange
            string userSub = Guid.NewGuid().ToString();
            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };
            
            var group = new Domain.Entities.Group { Admin = user, Name = "A group" };

            var userToGroup = new UserToGroup { User = user, Group = group };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupMembersValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetGroupMembers(userSub, group.Id));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}