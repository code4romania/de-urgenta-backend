using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetBackpacksValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpacksValidatorShould(DatabaseFixture fixture)
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
            var sut = new GetBackpacksValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new GetBackpacks(sub));

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_user_was_found_by_sub()
        {
            var sut = new GetBackpacksValidator(_dbContext);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var entity = new UserBuilder().WithSub(userSub).Build();
            await _dbContext.Users.AddAsync(entity);

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new GetBackpacks(userSub));

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}