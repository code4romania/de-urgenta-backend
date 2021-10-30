using System;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Options;
using DeUrgenta.Invite.Api.Validators;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AcceptBackpackInviteValidatorShould
    {
        private readonly DeUrgentaContext _context;
        private readonly IOptions<BackpacksConfig> _backpacksConfig;
        private readonly IamI18nProvider _i18nProvider;

        public AcceptBackpackInviteValidatorShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            var options = new BackpacksConfig
            {
                MaxContributors = 2
            };
            _backpacksConfig = Microsoft.Extensions.Options.Options.Create(options);

            _i18nProvider = Substitute.For<IamI18nProvider>();
            _i18nProvider.Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");
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

            var sut = new AcceptBackpackInviteValidator(_context, _i18nProvider, _backpacksConfig);

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

            var sut = new AcceptBackpackInviteValidator(_context, _i18nProvider, _backpacksConfig);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<DetailedValidationError>();

            await _i18nProvider.Received(1).Localize(Arg.Is("cannot-accept-invite"));
            await _i18nProvider.Received(1).Localize(Arg.Is("already-backpack-contributor"));
        }

        [Fact]
        public async Task Invalidate_request_if_backpack_already_has_too_many_contributors()
        {
            //Arrange
            var sub = Guid.NewGuid().ToString();
            var backpackId = Guid.NewGuid();

            var user = new UserBuilder().WithSub(sub).Build();
            await _context.Users.AddAsync(user);

            var backpack = new BackpackBuilder().WithId(backpackId).Build();
            await _context.Backpacks.AddAsync(backpack);

            var backpackToUser = new BackpackToUserBuilder().WithBackpack(backpack).Build();
            await _context.BackpacksToUsers.AddAsync(backpackToUser);
            var anotherBackpackToUser = new BackpackToUserBuilder().WithBackpack(backpack).Build();
            await _context.BackpacksToUsers.AddAsync(anotherBackpackToUser);

            await _context.SaveChangesAsync();

            AcceptBackpackInvite request = new(sub, backpackId);

            var sut = new AcceptBackpackInviteValidator(_context, _i18nProvider, _backpacksConfig);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<DetailedValidationError>();

            await _i18nProvider.Received(1).Localize(Arg.Is("cannot-accept-invite"));
            await _i18nProvider.Received(1).Localize(Arg.Is("max-backpack-contributors-reached"));
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

            var sut = new AcceptBackpackInviteValidator(_context, _i18nProvider, _backpacksConfig);

            //Act
            var isValid = await sut.IsValidAsync(request);

            //Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }
    }
}
