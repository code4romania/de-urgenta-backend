using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Validators;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection("Database collection")]
    public class DeleteSafeLocationValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public DeleteSafeLocationValidatorShould(DatabaseFixture fixture)
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
            var sut = new DeleteSafeLocationValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new DeleteSafeLocation(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_no_safe_location_found()
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

            var sut = new DeleteSafeLocationValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new DeleteSafeLocation(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_group_does_not_exists()
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
            var sut = new DeleteSafeLocationValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new DeleteSafeLocation(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_admin_of_group()
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

            var group = new Domain.Entities.Group
            {
                Admin = adminUser,
                Name = "a group"
            };

            var groupSafeLocation = new GroupSafeLocation
            {
                Group = group,
                Name = "A safe location"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(adminUser);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.GroupsSafeLocations.AddAsync(groupSafeLocation);
            await _dbContext.SaveChangesAsync();

            var sut = new DeleteSafeLocationValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new DeleteSafeLocation(userSub, groupSafeLocation.Id));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_requested_group()
        {
            // Arrange
            var sut = new DeleteSafeLocationValidator(_dbContext);

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

            var groupSafeLocation = new GroupSafeLocation
            {
                Group = group,
                Name = "A safe location"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.GroupsSafeLocations.AddAsync(groupSafeLocation);
            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new DeleteSafeLocation(userSub, groupSafeLocation.Id));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}