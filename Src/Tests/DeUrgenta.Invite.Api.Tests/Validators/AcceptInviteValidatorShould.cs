using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AcceptInviteValidatorShould
    {
        private readonly DeUrgentaContext _context;
        private readonly IServiceProvider _serviceProvider;

        private Dictionary<string, string> _config = new();

        public AcceptInviteValidatorShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;

            _config.Add("Groups:MaxJoinedGroupsPerUser", "2");
            _config.Add("Groups:UsersLimit", "2");

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(_config)
                .Build();

            _serviceProvider = new ServiceCollection()
                .AddSingleton(x => _context)
                .AddInviteApiServices(configuration)

                .BuildServiceProvider();
        }

        [Fact]
        public async Task Invalidate_request_if_users_does_not_exist()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();
            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_request_if_invite_does_not_exist()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_request_if_invite_is_already_accepted()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var invite = new InviteBuilder().WithStatus(InviteStatus.Accepted).WithId(inviteId).Build();
            await _context.Invites.AddAsync(invite);

            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_request_if_group_does_not_exist()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var invite = new InviteBuilder().WithType(InviteType.Group).WithId(inviteId).WithDestinationId(groupId).Build();
            await _context.Invites.AddAsync(invite);

            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Invalidate_request_if_user_is_already_a_member_of_too_many_groups()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            var userToGroups = new UserToGroupBuilder().WithGroupId(Guid.NewGuid()).WithUser(user).Build();
            await _context.UsersToGroups.AddAsync(userToGroups);
            var secondUserToGroups = new UserToGroupBuilder().WithGroupId(Guid.NewGuid()).WithUser(user).Build();
            await _context.UsersToGroups.AddAsync(secondUserToGroups);

            var invite = new InviteBuilder().WithId(inviteId).WithType(InviteType.Group).WithDestinationId(groupId).Build();
            await _context.Invites.AddAsync(invite);

            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeFalse();

        }

        [Fact]
        public async Task Invalidate_request_if_group_already_has_too_many_members()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            var userToGroups = new UserToGroupBuilder().WithGroup(group).Build();
            await _context.UsersToGroups.AddAsync(userToGroups);
            var secondUserToGroups = new UserToGroupBuilder().WithGroup(group).Build();
            await _context.UsersToGroups.AddAsync(secondUserToGroups);

            var invite = new InviteBuilder().WithType(InviteType.Group).WithId(inviteId).WithDestinationId(groupId).Build();
            await _context.Invites.AddAsync(invite);

            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Validate_request_if_accept_group_invite_request_is_valid()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            var invite = new InviteBuilder().WithType(InviteType.Group).WithId(inviteId).WithDestinationId(groupId).Build();
            await _context.Invites.AddAsync(invite);

            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public async Task Invalidate_request_if_backpack_does_not_exist()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();
            var backpackId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var invite = new InviteBuilder().WithType(InviteType.Backpack).WithId(inviteId).WithDestinationId(backpackId).Build();
            await _context.Invites.AddAsync(invite);

            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Validate_request_if_accept_backpack_invite_request_is_valid()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();
            var backpackId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var backpack = new BackpackBuilder().WithId(backpackId).Build();
            await _context.Backpacks.AddAsync(backpack);

            var invite = new InviteBuilder().WithType(InviteType.Backpack).WithId(inviteId).WithDestinationId(backpackId).Build();
            await _context.Invites.AddAsync(invite);

            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeTrue();
        }
    }
}
