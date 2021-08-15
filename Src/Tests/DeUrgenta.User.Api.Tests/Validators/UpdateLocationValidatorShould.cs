using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Validators;
using Shouldly;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateLocationValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateLocationValidatorShould(DatabaseFixture fixture)
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
            var sut = new UpdateLocationValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateLocation(sub, Guid.NewGuid(), new UserLocationRequest()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_user_was_found_by_sub_and_location_does_not_exists()
        {
            var sut = new UpdateLocationValidator(_dbContext);

            // Arrange
            string userSub = Guid.NewGuid().ToString();
            await _dbContext.Users.AddAsync(new DeUrgenta.Domain.Entities.User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            });

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateLocation(userSub, Guid.NewGuid(), new UserLocationRequest()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_user_was_found_by_sub_and_location_exists()
        {
            var sut = new UpdateLocationValidator(_dbContext);

            // Arrange
            string userSub = Guid.NewGuid().ToString();

            var user = new DeUrgenta.Domain.Entities.User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };
            await _dbContext.Users.AddAsync(user);

            var userLocation = new UserLocation
            {
                User = user,
                Address = "Splaiul Unirii 160, 040041 Bucharest, Romania",
                Type = UserLocationType.Work,
                Longitude = 44.41184746891321m,
                Latitude = 26.118310383230373m

            };
            await _dbContext.UserLocations.AddAsync(userLocation);

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateLocation(userSub, userLocation.Id, new UserLocationRequest()));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}