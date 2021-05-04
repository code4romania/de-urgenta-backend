using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.Tests.Helpers;
using Shouldly;
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
            bool isValid = await sut.IsValidAsync(new GetGroupSafeLocations(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_no_group_found()
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

            var sut = new GetGroupSafeLocationsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetGroupSafeLocations(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_not_part_of_the_group()
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

            var admin = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Sub = adminSub
            };

            var group = new Domain.Entities.Group
            {
                Admin = admin,
                Name = "A group"
            };

            var adminToGroup = new UserToGroup { Group = group, User = admin };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(adminToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupSafeLocationsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetGroupSafeLocations(userSub, group.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_request_when_is_admin_of_group()
        {
            // Arrange
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
                Name = "A group"
            };
            var userToGroup = new UserToGroup { Group = group, User = user };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupSafeLocationsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetGroupSafeLocations(userSub, group.Id));

            // Assert
            isValid.ShouldBeTrue();
        }

        [Fact]
        public async Task Validate_request_when_is_part_of_group()
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

            var admin = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Sub = adminSub
            };

            var group = new Domain.Entities.Group
            {
                Admin = admin,
                Name = "A group"
            };

            var userToGroup = new UserToGroup { Group = group, User = user };
            var adminToGroup = new UserToGroup { Group = group, User = admin };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(userToGroup);
            await _dbContext.UsersToGroups.AddAsync(adminToGroup);

            await _dbContext.SaveChangesAsync();

            var sut = new GetGroupSafeLocationsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetGroupSafeLocations(userSub, group.Id));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}