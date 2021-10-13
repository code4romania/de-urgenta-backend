using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class CreateBackpackValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public CreateBackpackValidatorShould(DatabaseFixture fixture)
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
            var sut = new CreateBackpackValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new CreateBackpack(sub, new BackpackModelRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_user_was_found_by_sub()
        {
            var sut = new CreateBackpackValidator(_dbContext);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();
            
            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new CreateBackpack(userSub, new BackpackModelRequest()));

            // Assert
            isValid.Should().BeOfType<ValidationPassedResult>();
        }
    }
}