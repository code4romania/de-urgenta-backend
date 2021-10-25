using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using NSubstitute;
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
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sut = new DeleteBackpackValidator(_dbContext, i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new DeleteBackpack(sub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_backpack_does_not_exists()
        {
            // Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            var sut = new DeleteBackpackValidator(_dbContext, i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new DeleteBackpack(userSub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_owner_of_backpack()
        {
            // Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var userSub = Guid.NewGuid().ToString();
            var ownerSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();
            var owner = new UserBuilder().WithSub(ownerSub).Build();

            var backpack = new Domain.Api.Entities.Backpack
            {
                Name = "A backpack"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = user, IsOwner = false });
            await _dbContext.SaveChangesAsync();

            var sut = new DeleteBackpackValidator(_dbContext, i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new DeleteBackpack(userSub, backpack.Id));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_user_is_owner_of_requested_backpack()
        {
            // Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sut = new DeleteBackpackValidator(_dbContext, i18nProvider);

            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var backpack = new Domain.Api.Entities.Backpack
            {
                Name = "my backpack"
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = user, IsOwner = true });
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new DeleteBackpack(userSub, backpack.Id));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}