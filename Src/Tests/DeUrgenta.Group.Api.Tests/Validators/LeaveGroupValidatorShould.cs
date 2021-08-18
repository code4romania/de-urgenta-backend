using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.Tests.Helpers;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class LeaveGroupValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public LeaveGroupValidatorShould(DatabaseFixture fixture)
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
            var sut = new LeaveGroupValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new LeaveGroup(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_no_group_found()
        {
            // Arrange
            var sut = new LeaveGroupValidator(_dbContext);

            string userSub = Guid.NewGuid().ToString();

            await _dbContext.Users.AddAsync(new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            });


            // Act
            bool isValid = await sut.IsValidAsync(new LeaveGroup(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_user_not_part_of_group()
        {
            // Arrange
            var sut = new LeaveGroupValidator(_dbContext);
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

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new LeaveGroup(userSub, group.Id));

            // Assert
            isValid.ShouldBeFalse();
        }


        [Fact]
        public async Task Invalidate_when_is_admin_of_group()
        {
            // Arrange
            var sut = new LeaveGroupValidator(_dbContext);
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
            bool isValid = await sut.IsValidAsync(new LeaveGroup(userSub, group.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_is_part_of_requested_group()
        {
            // Arrange
            var sut = new LeaveGroupValidator(_dbContext);
            string userSub = Guid.NewGuid().ToString();
            string adminSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            var admin = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = adminSub
            };

            var group = new Domain.Entities.Group
            {
                Admin = admin,
                Name = "my group"
            };

            var userToGroups = new UserToGroup { Group = group, User = user };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroups);
            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new LeaveGroup(userSub, group.Id));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}