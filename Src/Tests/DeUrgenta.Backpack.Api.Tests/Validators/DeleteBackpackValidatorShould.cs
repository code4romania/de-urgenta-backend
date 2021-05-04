using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using Shouldly;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class DeleteBackpackValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public DeleteBackpackValidatorShould(DatabaseFixture fixture)
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
            var sut = new DeleteBackpackValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new DeleteBackpack(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }
    }
}