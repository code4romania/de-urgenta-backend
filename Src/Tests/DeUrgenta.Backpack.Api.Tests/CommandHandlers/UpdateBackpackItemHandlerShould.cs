using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.CommandHandlers;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateBackpackItemHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateBackpackItemHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Successfully_update_backpack_item()
        {
            // Arrange
            var validator = new UpdateBackpackItemValidator(_dbContext);
            var sut = new UpdateBackpackItemHandler(validator, _dbContext);

            var user = new UserBuilder().Build();
            var backpack = new BackpackBuilder().Build();
            var backpackToUser = new BackpackToUser {Backpack = backpack, User = user};
            var backpackItem = new BackpackItemBuilder().WithBackpack(backpack).Build();
            backpack.BackpackItems.Add(backpackItem);
            await _dbContext.BackpacksToUsers.AddAsync(backpackToUser);
            await _dbContext.SaveChangesAsync();

            // First assertion
            _dbContext.BackpackItems.Where(i => i.Id.Equals(backpackItem.Id)).First().Version.Should().Be(0);
            
            // Act
            var result = await sut.Handle(
                new UpdateBackpackItem(
                    user.Sub,
                    backpackItem.Id,
                    new BackpackItemRequest() {Name = "test", Version = 0}
                ),
                CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be("test");
            result.Value.Version.Should().Be(1);
        }
        
        [Fact]
        public async Task Fail_to_update_using_outdated_model()
        {
            // Arrange
            var validator = new UpdateBackpackItemValidator(_dbContext);
            var sut = new UpdateBackpackItemHandler(validator, _dbContext);

            var user = new UserBuilder().Build();
            var backpack = new BackpackBuilder().Build();
            var backpackToUser = new BackpackToUser {Backpack = backpack, User = user};
            var backpackItem = new BackpackItemBuilder().WithBackpack(backpack).Build();
            backpack.BackpackItems.Add(backpackItem);
            await _dbContext.BackpacksToUsers.AddAsync(backpackToUser);
            await _dbContext.SaveChangesAsync();

            // First assertion
            _dbContext.BackpackItems.Where(i => i.Id.Equals(backpackItem.Id)).First().Version.Should().Be(0);
            
            // Act
            var result = await sut.Handle(
                new UpdateBackpackItem(
                    user.Sub,
                    backpackItem.Id,
                    new BackpackItemRequest() {Name = "test", Version = 4}
                ),
                CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<UpdateBackpackItem>>();
            validator
                .IsValidAsync(Arg.Any<UpdateBackpackItem>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new UpdateBackpackItemHandler(validator, _dbContext);

            // Act
            var result =
                await sut.Handle(new UpdateBackpackItem("a-sub", Guid.NewGuid(), new Models.BackpackItemRequest()),
                    CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}