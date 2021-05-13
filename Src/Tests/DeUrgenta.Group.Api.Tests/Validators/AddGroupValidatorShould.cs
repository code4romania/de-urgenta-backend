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
    public class AddGroupValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public AddGroupValidatorShould(DatabaseFixture fixture)
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
            var sut = new AddGroupValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new AddGroup(sub, new GroupRequest()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_when_user_was_found_by_sub()
        {
            var sut = new AddGroupValidator(_dbContext);

            // Arrange
            string userSub = Guid.NewGuid().ToString();
            await _dbContext.Users.AddAsync(new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            });

            await _dbContext.SaveChangesAsync();
            
            // Act
            bool isValid = await sut.IsValidAsync(new AddGroup(userSub, new GroupRequest()));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}