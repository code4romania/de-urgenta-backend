using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Options;
using DeUrgenta.User.Api.Validators;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AcceptGroupInviteValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IOptions<GroupsConfig> _defaultOptions = Substitute.For<IOptions<GroupsConfig>>();

        public AcceptGroupInviteValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            _defaultOptions.Value.Returns(new GroupsConfig {UsersLimit = 30});
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("my-weird-sub")]
        public async Task Invalidate_request_when_no_user_found_by_sub(string sub)
        {
            // Arrange
            var sut = new AcceptGroupInviteValidator(_dbContext, _defaultOptions);

            // Act
            var isValid = await sut.IsValidAsync(new AcceptGroupInvite(sub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_invite_is_for_other_user()
        {
            var sut = new AcceptGroupInviteValidator(_dbContext, _defaultOptions);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var otherUserSub = Guid.NewGuid().ToString();
            var otherUser = new UserBuilder().WithSub(otherUserSub).Build();

            var adminSub = Guid.NewGuid().ToString();
            var admin = new UserBuilder().WithSub(adminSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(admin);
            await _dbContext.Users.AddAsync(otherUser);

            var group = new GroupBuilder().WithAdmin(admin).Build();

            await _dbContext.Groups.AddAsync(group);

            var groupInvite = new GroupInvite {InvitationReceiver = user, InvitationSender = admin, Group = group};

            await _dbContext.GroupInvites.AddAsync(groupInvite);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new AcceptGroupInvite(userSub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_invite_does_not_exist()
        {
            var sut = new AcceptGroupInviteValidator(_dbContext, _defaultOptions);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var adminSub = Guid.NewGuid().ToString();
            var admin = new UserBuilder().WithSub(adminSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(admin);

            var group = new GroupBuilder().WithAdmin(admin).Build();

            await _dbContext.Groups.AddAsync(group);

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new AcceptGroupInvite(userSub, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_exceeds_group_users_limit()
        {
            var options = Substitute.For<IOptions<GroupsConfig>>();
            options.Value.Returns(new GroupsConfig {UsersLimit = 2});
            var sut = new AcceptGroupInviteValidator(_dbContext, options);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var secondUserSub = Guid.NewGuid().ToString();
            var secondUser = new UserBuilder().WithSub(secondUserSub).Build();

            var adminSub = Guid.NewGuid().ToString();
            var admin = new UserBuilder().WithSub(adminSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(admin);

            var group = new GroupBuilder().WithAdmin(admin).Build();

            await _dbContext.UsersToGroups.AddAsync(new UserToGroup {Group = group, User = admin});
            await _dbContext.UsersToGroups.AddAsync(new UserToGroup {Group = group, User = user});

            var groupInvite = new GroupInvite
            {
                InvitationReceiver = secondUser, InvitationSender = admin, Group = group
            };
            await _dbContext.GroupInvites.AddAsync(groupInvite);
            await _dbContext.Groups.AddAsync(group);

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new AcceptGroupInvite(secondUserSub, groupInvite.Id));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_an_invite_exists()
        {
            var sut = new AcceptGroupInviteValidator(_dbContext, _defaultOptions);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            var adminSub = Guid.NewGuid().ToString();
            var admin = new UserBuilder().WithSub(adminSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Users.AddAsync(admin);

            var group = new GroupBuilder().WithAdmin(admin).Build();

            var groupInvite = new GroupInvite {InvitationReceiver = user, InvitationSender = admin, Group = group};
            await _dbContext.GroupInvites.AddAsync(groupInvite);
            await _dbContext.Groups.AddAsync(group);

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new AcceptGroupInvite(userSub, groupInvite.Id));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}