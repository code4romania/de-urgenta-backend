using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class InviteToBackpackContributorsValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public InviteToBackpackContributorsValidatorShould(DatabaseFixture fixture)
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
            var sut = new InviteToBackpackContributorsValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new InviteToBackpackContributors(sub, Guid.NewGuid(), Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_user_adds_himself_to_contributors()
        {
            // Arrange
            var sut = new InviteToBackpackContributorsValidator(_dbContext);
            var userSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = user, IsOwner = true });
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new InviteToBackpackContributors(userSub, backpack.Id, user.Id));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_backpack_not_found()
        {
            // Arrange
            var sut = new InviteToBackpackContributorsValidator(_dbContext);
            var userSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new InviteToBackpackContributors(userSub, Guid.NewGuid(), Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_invited_user_not_found()
        {
            // Arrange
            var sut = new InviteToBackpackContributorsValidator(_dbContext);
            var userSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithSub(userSub).Build();

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new InviteToBackpackContributors(userSub, backpack.Id, Guid.NewGuid()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_when_user_is_already_invited()
        {
            // Arrange
            var sut = new InviteToBackpackContributorsValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var nonContributorSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithSub(userSub).Build();
            var invitedContributor = new UserBuilder().WithSub(nonContributorSub).Build();

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            var backpackInvite = new BackpackInvite
            {
                Backpack = backpack,
                InvitationReceiver = invitedContributor,
                InvitationSender = owner
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Users.AddAsync(invitedContributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });

            await _dbContext.BackpackInvites.AddAsync(backpackInvite);

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new InviteToBackpackContributors(userSub, backpackInvite.Id, invitedContributor.Id));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_when_user_is_owner_of_backpack_and_invited_existing_user()
        {
            // Arrange
            var sut = new InviteToBackpackContributorsValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var nonContributorSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithSub(userSub).Build();
            var nonContributor = new UserBuilder().WithSub(nonContributorSub).Build();

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Users.AddAsync(nonContributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = owner, IsOwner = true });
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = nonContributor, IsOwner = false });


            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new InviteToBackpackContributors(userSub, backpack.Id, nonContributor.Id));

            // Assert
            isValid.Should().BeOfType<ValidationPassedResult>();
        }

        [Fact]
        public async Task Validate_when_user_is_contributor_and_invited_existing_user()
        {
            // Arrange
            var sut = new InviteToBackpackContributorsValidator(_dbContext);

            var userSub = Guid.NewGuid().ToString();
            var nonContributorSub = Guid.NewGuid().ToString();

            var contributor = new UserBuilder().WithSub(userSub).Build();
            var nonContributor = new UserBuilder().WithSub(nonContributorSub).Build();

            var backpack = new Domain.Entities.Backpack
            {
                Name = "my backpack"
            };

            await _dbContext.Users.AddAsync(contributor);
            await _dbContext.Users.AddAsync(nonContributor);
            await _dbContext.Backpacks.AddAsync(backpack);
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = contributor, IsOwner = true });
            await _dbContext.BackpacksToUsers.AddAsync(new BackpackToUser { Backpack = backpack, User = nonContributor, IsOwner = false });

            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new InviteToBackpackContributors(userSub, backpack.Id, nonContributor.Id));

            // Assert
            isValid.Should().BeOfType<ValidationPassedResult>();
        }
    }
}