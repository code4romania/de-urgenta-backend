using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AcceptInviteValidatorShould
    {
        private readonly DeUrgentaContext _context;

        public AcceptInviteValidatorShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;

        }

        [Fact]
        public async Task Invalidate_request_if_users_does_not_exist()
        {
            //Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();
            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, i18nProvider);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_if_invite_does_not_exist()
        {
            //Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, i18nProvider);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_if_invite_is_already_accepted()
        {
            //Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var invite = new InviteBuilder().WithStatus(InviteStatus.Accepted).WithId(inviteId).Build();
            await _context.Invites.AddAsync(invite);

            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, i18nProvider);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_request_if_request_is_valid()
        {
            //Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var invite = new InviteBuilder().WithId(inviteId).Build();
            await _context.Invites.AddAsync(invite);

            await _context.SaveChangesAsync();

            AcceptInvite request = new(sub, inviteId);

            var sut = new AcceptInviteValidator(_context, i18nProvider);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}
