using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.CommandHandlers;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Options;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.CommandsHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AddGroupHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IOptions<GroupsConfig> _groupsConfig;

        public AddGroupHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            var options = new GroupsConfig {UsersLimit = 35};
            _groupsConfig = Microsoft.Extensions.Options.Options.Create(options);
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<AddGroup>>();
            validator
                .IsValidAsync(Arg.Any<AddGroup>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new AddGroupHandler(validator, _dbContext, _groupsConfig);

            // Act
            var result = await sut.Handle(new AddGroup("a-sub", new GroupRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Return_max_number_of_users()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<AddGroup>>();
            validator
                .IsValidAsync(Arg.Any<AddGroup>())
                .Returns(Task.FromResult(ValidationResult.Ok));

            var userId = Guid.NewGuid();
            var userSub = TestDataProviders.RandomString();
            var user = new UserBuilder().WithId(userId).WithSub(userSub).Build();
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var sut = new AddGroupHandler(validator, _dbContext, _groupsConfig);

            // Act
            var result =
                await sut.Handle(new AddGroup(userSub, new GroupRequest {Name = TestDataProviders.RandomString()}),
                    CancellationToken.None);

            // Assert
            result.Value.MaxNumberOfMembers.Should().Be(35);
        }
    }
}