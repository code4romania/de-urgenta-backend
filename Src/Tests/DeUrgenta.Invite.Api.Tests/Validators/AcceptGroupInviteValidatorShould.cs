using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Options;
using DeUrgenta.Invite.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AcceptGroupInviteValidatorShould
    {
        private readonly DeUrgentaContext _context;
        private readonly IOptions<GroupsConfig> _groupsConfig;

        public AcceptGroupInviteValidatorShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            var options = new GroupsConfig
            {
                MaxUsers = 2,
                MaxJoinedGroupsPerUser = 2
            };
            _groupsConfig = Microsoft.Extensions.Options.Options.Create(options);
        }

        [Fact]
        public async Task Invalidate_request_if_group_does_not_exist()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);
            
            await _context.SaveChangesAsync();

            AcceptGroupInvite request = new(sub, groupId);

            var sut = new AcceptGroupInviteValidator(_context, _groupsConfig);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_if_user_is_already_a_member_of_too_many_groups()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            var userToGroups = new UserToGroupBuilder().WithGroupId(Guid.NewGuid()).WithUser(user).Build();
            await _context.UsersToGroups.AddAsync(userToGroups);
            var secondUserToGroups = new UserToGroupBuilder().WithGroupId(Guid.NewGuid()).WithUser(user).Build();
            await _context.UsersToGroups.AddAsync(secondUserToGroups);

            await _context.SaveChangesAsync();

            AcceptGroupInvite request = new(sub, groupId);

            var sut = new AcceptGroupInviteValidator(_context, _groupsConfig);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();

        }

        [Fact]
        public async Task Invalidate_request_if_group_already_has_too_many_members()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            var userToGroups = new UserToGroupBuilder().WithGroup(group).Build();
            await _context.UsersToGroups.AddAsync(userToGroups);
            var secondUserToGroups = new UserToGroupBuilder().WithGroup(group).Build();
            await _context.UsersToGroups.AddAsync(secondUserToGroups);

            await _context.SaveChangesAsync();

            AcceptGroupInvite request = new(sub, groupId);

            var sut = new AcceptGroupInviteValidator(_context, _groupsConfig);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_if_user_already_a_group_member()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            var userToGroups = new UserToGroupBuilder().WithGroup(group).WithUser(user).Build();
            await _context.UsersToGroups.AddAsync(userToGroups);
            
            await _context.SaveChangesAsync();

            AcceptGroupInvite request = new(sub, groupId);

            var sut = new AcceptGroupInviteValidator(_context, _groupsConfig);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_request_if_accept_group_invite_request_is_valid()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            await _context.SaveChangesAsync();

            AcceptGroupInvite request = new(sub, groupId);

            var sut = new AcceptGroupInviteValidator(_context, _groupsConfig);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}
