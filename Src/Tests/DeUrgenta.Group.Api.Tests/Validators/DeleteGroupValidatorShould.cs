using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Validators;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class DeleteGroupValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IamI18nProvider _i18nProvider;

        public DeleteGroupValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            _i18nProvider = Substitute.For<IamI18nProvider>();
            _i18nProvider.Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("my-weird-sub")]
        public async Task Invalidate_request_when_no_user_found_by_sub(string sub)
        {
            // Arrange
            var sut = new DeleteGroupValidator(_dbContext, _i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new DeleteGroup(sub, Guid.NewGuid()));

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

            var sut = new DeleteGroupValidator(_dbContext, _i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new DeleteGroup(userSub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_part_of_group()
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

            var sut = new DeleteGroupValidator(_dbContext, _i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new DeleteGroup(userSub, group.Id));

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
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup
            {
                Group = group,
                User = user
            });

            await _dbContext.SaveChangesAsync();

            var sut = new DeleteGroupValidator(_dbContext, _i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new DeleteGroup(userSub, group.Id));

            // Assert
            isValid.Should().BeOfType<DetailedValidationError>();

            await _i18nProvider.Received(1).Localize(Arg.Is("cannot-delete-group"));
            await _i18nProvider.Received(1).Localize(Arg.Is("only-group-admin-can-delete-group-message"));
        }

        [Fact]
        public async Task Validate_when_user_is_admin_of_requested_group()
        {
            // Arrange
            var sut = new DeleteGroupValidator(_dbContext, _i18nProvider);

            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var group = new GroupBuilder().WithAdmin(user).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup
            {
                Group = group,
                User = user
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new DeleteGroup(userSub, group.Id));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}