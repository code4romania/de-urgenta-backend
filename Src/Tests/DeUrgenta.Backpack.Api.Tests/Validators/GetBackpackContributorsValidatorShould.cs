using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using Shouldly;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetBackpackContributorsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpackContributorsValidatorShould(DatabaseFixture fixture)
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
            var sut = new GetBackpackContributorsValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new GetBackpackContributors(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }
    }
}