using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AddBackpackItemValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        public AddBackpackItemValidatorShould(DatabaseFixture fixture)
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
            var sut = new AddBackpackItemValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new AddBackpackItem(sub, Guid.NewGuid(), new BackpackItemRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_user_not_contributor_of_related_backpack()
        {
            // Arrange
            var sut = new AddBackpackItemValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var contributorSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var nonContributor = new UserBuilder().WithSub(userSub).Build();
            var contributor = new UserBuilder().WithSub(contributorSub).Build();
           
            var backpack = new Domain.Entities.Backpack
            {
                Id = backpackId,
                Name = "A backpack"
            };

            await _dbContext.Users.AddAsync(nonContributor);
            await _dbContext.Users.AddAsync(contributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = contributor});
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new AddBackpackItem(nonContributor.Sub, backpackId, new BackpackItemRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }


        [Fact]
        public async Task Validate_request_when_backpack_exists_and_user_contributor()
        {
            // Arrange
            var sut = new AddBackpackItemValidator(_dbContext);

            var contributorSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var contributor = new UserBuilder().WithSub(contributorSub).Build();

            var backpack = new Domain.Entities.Backpack
            {
                Id = backpackId,
                Name = "test-backpack",
            };

            await _dbContext.Users.AddAsync(contributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = contributor });
            await _dbContext.SaveChangesAsync();

            var command = new AddBackpackItem(contributorSub, backpackId, new BackpackItemRequest());

            // Act
            var isValid = await sut.IsValidAsync(command);

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}
