using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using DeUrgenta.Invite.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using InviteType = DeUrgenta.Invite.Api.Models.InviteType;

namespace DeUrgenta.Invite.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class CreateInviteValidatorShould
    {
        private readonly DeUrgentaContext _context;
        private readonly IServiceProvider _serviceProvider;

        private Dictionary<string, string> _config = new();

        public CreateInviteValidatorShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;

            _config.Add("Groups:MaxUsers", "2");

            IConfiguration configuration =  new ConfigurationBuilder()
                    .AddInMemoryCollection(_config)
                    .Build();

            _serviceProvider = new ServiceCollection()
                    .AddSingleton(_ => _context)
                    .AddInviteApiServices(configuration)
                    .BuildServiceProvider();
        }

        [Fact]
        public async Task Invalidate_request_if_users_does_not_exist()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var inviteRequest = new InviteRequest
            {
                Type = InviteType.Group,
                DestinationId = Guid.NewGuid()
            };
            CreateInvite request = new(sub, inviteRequest);

            var sut = new CreateInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_if_group_does_not_exist()
        {
            //Arrange
            var userSub = Guid.NewGuid().ToString();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(userSub).Build();
            await _context.Users.AddAsync(user);
            
            await _context.SaveChangesAsync();

            var inviteRequest = new InviteRequest
            {
                Type = InviteType.Group,
                DestinationId = groupId
            };
            CreateInvite request = new(userSub, inviteRequest);

            var sut = new CreateInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_if_user_generating_invite_does_not_belong_to_group()
        {
            //Arrange
            var userSub = Guid.NewGuid().ToString();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(userSub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            var userToGroups = new UserToGroupBuilder().WithGroupId(groupId).Build();
            await _context.UsersToGroups.AddAsync(userToGroups);

            await _context.SaveChangesAsync();

            var inviteRequest = new InviteRequest
            {
                Type = InviteType.Group,
                DestinationId = groupId
            };
            CreateInvite request = new(userSub, inviteRequest);

            var sut = new CreateInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_if_group_already_has_maximum_number_of_users()
        {
            //Arrange
            var userSub = Guid.NewGuid().ToString();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(userSub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            var userToGroups = new UserToGroupBuilder().WithGroup(group).WithUser(user).Build();
            await _context.UsersToGroups.AddAsync(userToGroups);
            var secondUserToGroups = new UserToGroupBuilder().WithGroup(group).Build();
            await _context.UsersToGroups.AddAsync(secondUserToGroups);

            await _context.SaveChangesAsync();

            var inviteRequest = new InviteRequest
            {
                Type = InviteType.Group,
                DestinationId = groupId
            };
            CreateInvite request = new(userSub, inviteRequest);

            var sut = new CreateInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_request_when_group_invite_request_is_valid()
        {
            //Arrange
            var userSub = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var user = new UserBuilder().WithId(userId).WithSub(userSub).Build();
            await _context.Users.AddAsync(user);

            var group = new GroupBuilder().WithId(groupId).Build();
            await _context.Groups.AddAsync(group);

            var userToGroups = new UserToGroupBuilder()
                .WithGroup(group)
                .WithUser(user)
                .Build();
            await _context.UsersToGroups.AddAsync(userToGroups);

            await _context.SaveChangesAsync();

            var inviteRequest = new InviteRequest
            {
                Type = InviteType.Group,
                DestinationId = groupId
            };
            CreateInvite request = new(userSub, inviteRequest);

            var sut = new CreateInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }

        [Fact]
        public async Task Invalidate_request_if_backpack_does_not_exist()
        {
            //Arrange
            var userSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(userSub).Build();

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var inviteRequest = new InviteRequest
            {
                Type = InviteType.Backpack,
                DestinationId = backpackId
            };
            CreateInvite request = new(userSub, inviteRequest);

            var sut = new CreateInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_if_user_generating_invite_does_not_belong_to_backpack()
        {
            //Arrange
            var userSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(userSub).WithId(userId).Build();
            await _context.Users.AddAsync(user);

            var backpack = new BackpackBuilder().WithId(backpackId).Build();
            await _context.Backpacks.AddAsync(backpack);

            var backpackToUser = new BackpackToUserBuilder().WithBackpack(backpack).Build();
            await _context.BackpacksToUsers.AddAsync(backpackToUser);

            await _context.SaveChangesAsync();

            var inviteRequest = new InviteRequest
            {
                Type = InviteType.Backpack,
                DestinationId = backpackId
            };
            CreateInvite request = new(userSub, inviteRequest);

            var sut = new CreateInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_request_when_backpack_invite_request_is_valid()
        {
            //Arrange
            var userSub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(userSub).WithId(userId).Build();
            await _context.Users.AddAsync(user);

            var backpack = new BackpackBuilder().WithId(backpackId).Build();
            await _context.Backpacks.AddAsync(backpack);

            var backpackToUser = new BackpackToUserBuilder().WithBackpack(backpack).WithUser(user).Build();
            await _context.BackpacksToUsers.AddAsync(backpackToUser);

            await _context.SaveChangesAsync();

            var inviteRequest = new InviteRequest
            {
                Type = InviteType.Backpack,
                DestinationId = backpackId
            };
            CreateInvite request = new(userSub, inviteRequest);

            var sut = new CreateInviteValidator(_context, new InviteValidatorFactory(_serviceProvider));

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}
