using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AcceptBackpackInviteValidatorShould
    {
        private readonly DeUrgentaContext _context;

        public AcceptBackpackInviteValidatorShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Invalidate_request_if_backpack_does_not_exist()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            AcceptBackpackInvite request = new(sub, backpackId);

            var sut = new AcceptBackpackInviteValidator(_context);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_if_user_already_a_backpack_contributor()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var backpack = new BackpackBuilder().WithId(backpackId).Build();
            await _context.Backpacks.AddAsync(backpack);

            var backpackToUser = new BackpackToUserBuilder().WithBackpack(backpack).WithUser(user).Build();
            await _context.BackpacksToUsers.AddAsync(backpackToUser);
            
            await _context.SaveChangesAsync();

            AcceptBackpackInvite request = new(sub, backpackId);

            var sut = new AcceptBackpackInviteValidator(_context);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_request_if_accept_backpack_invite_request_is_valid()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var backpack = new BackpackBuilder().WithId(backpackId).Build();
            await _context.Backpacks.AddAsync(backpack);

            await _context.SaveChangesAsync();

            AcceptBackpackInvite request = new(sub, backpackId);

            var sut = new AcceptBackpackInviteValidator(_context);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}
