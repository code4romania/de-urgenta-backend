using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Options;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AddSafeLocationValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IOptions<GroupsConfig> _config;
        private readonly IamI18nProvider _i18nProvider;

        public AddSafeLocationValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            _config = Substitute.For<IOptions<GroupsConfig>>();
            _config.Value.Returns(new GroupsConfig { MaxSafeLocations = 2 });

            _i18nProvider = Substitute.For<IamI18nProvider>();
            _i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("my-weird-sub")]
        public async Task Invalidate_request_when_no_user_found_by_sub(string sub)
        {
            // Arrange
            var sut = new AddSafeLocationValidator(_dbContext, _i18nProvider, _config);

            // Act
            var isValid = await sut.IsValidAsync(new AddSafeLocation(sub, Guid.NewGuid(), new SafeLocationRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_group_does_not_exists()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            var sut = new AddSafeLocationValidator(_dbContext, _i18nProvider, _config);

            // Act
            var isValid = await sut.IsValidAsync(new AddSafeLocation(userSub, Guid.NewGuid(), new SafeLocationRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_admin_of_group()
        {
            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var adminSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();
            var adminUser = new UserBuilder().WithSub(adminSub).Build();

            var group = new GroupBuilder().WithAdmin(adminUser).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(adminUser);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            var sut = new AddSafeLocationValidator(_dbContext, _i18nProvider, _config);

            // Act
            var isValid =
                await sut.IsValidAsync(new AddSafeLocation(userSub, group.Id, new SafeLocationRequest()));

            // Assert
            isValid.Should().BeOfType<DetailedValidationError>();

            await _i18nProvider.Received(1).Localize(Arg.Is("cannot-add-safe-location"));
            await _i18nProvider.Received(1).Localize(Arg.Is("only-group-admin-can-add-locations-message"));
        }

        [Fact]
        public async Task Invalidate_request_when_group_already_hax_max_safe_locations()
        {
            //Arrange
            var sut = new AddSafeLocationValidator(_dbContext, _i18nProvider, _config);

            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();
            await _dbContext.Users.AddAsync(user);

            var group = new GroupBuilder().WithAdmin(user).Build();
            await _dbContext.Groups.AddAsync(group);

            var safeLocation = new GroupSafeLocationBuilder().WithGroup(group).Build();
            var anotherSafeLocation = new GroupSafeLocationBuilder().WithGroup(group).Build();
            await _dbContext.GroupsSafeLocations.AddAsync(safeLocation);
            await _dbContext.GroupsSafeLocations.AddAsync(anotherSafeLocation);

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new AddSafeLocation(userSub, group.Id, new SafeLocationRequest()));

            // Assert
            isValid.Should().BeOfType<DetailedValidationError>();

            await _i18nProvider.Received(1).Localize(Arg.Is("group-safe-location-limit"));
            await _i18nProvider.Received(1).Localize(Arg.Is("group-safe-location-limit-message"));
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_requested_group()
        {
            // Arrange
            var sut = new AddSafeLocationValidator(_dbContext, _i18nProvider, _config);

            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var group = new GroupBuilder().WithAdmin(user).Build();
            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new AddSafeLocation(userSub, group.Id, new SafeLocationRequest()));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}