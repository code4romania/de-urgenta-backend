using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetAdministeredGroupsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetAdministeredGroupsValidatorShould(DatabaseFixture fixture)
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
            var sut = new GetAdministeredGroupsValidator(_dbContext);

            // Act
            var result = await sut.IsValidAsync(new GetAdministeredGroups(sub), CancellationToken.None);

            // Assert
            result.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_user_was_found_by_sub()
        {
            // Arrange
            var sut = new GetAdministeredGroupsValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new GetAdministeredGroups(userSub), CancellationToken.None);

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}