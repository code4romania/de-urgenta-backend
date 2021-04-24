using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Validators;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection("Database collection")]
    public class UpdateSafeLocationValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateSafeLocationValidatorShould(DatabaseFixture fixture)
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
            var sut = new UpdateSafeLocationValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateSafeLocation(sub, Guid.NewGuid(), Guid.NewGuid(), new SafeLocationRequest()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_user_was_found_by_sub()
        {
            // Arrange
            var sut = new UpdateSafeLocationValidator(_dbContext);
            string userSub = Guid.NewGuid().ToString();

            await _dbContext.Users.AddAsync(new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            });

            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateSafeLocation(userSub, Guid.NewGuid(), Guid.NewGuid(), new SafeLocationRequest()));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}