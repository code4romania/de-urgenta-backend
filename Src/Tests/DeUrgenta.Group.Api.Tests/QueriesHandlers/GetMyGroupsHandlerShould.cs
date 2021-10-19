using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Group.Api.Options;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.QueryHandlers;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.QueriesHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetMyGroupsHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IOptions<GroupsConfig> _groupsConfig;

        public GetMyGroupsHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            var options = new GroupsConfig {MaxUsers = 35};
            _groupsConfig = Microsoft.Extensions.Options.Options.Create(options);
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetMyGroups>>();
            validator
                .IsValidAsync(Arg.Any<GetMyGroups>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new GetMyGroupsHandler(validator, _dbContext, _groupsConfig);

            // Act
            var result = await sut.Handle(new GetMyGroups("a-sub"), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Return_max_number_of_users()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetMyGroups>>();
            validator
                .IsValidAsync(Arg.Any<GetMyGroups>())
                .Returns(Task.FromResult(ValidationResult.Ok));

            var userId = Guid.NewGuid();
            var userSub = TestDataProviders.RandomString();
            var user = new UserBuilder().WithId(userId).WithSub(userSub).Build();
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var group = new GroupBuilder().WithAdmin(user).Build();
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            var userToGroup = new UserToGroup {User = user, Group = group};
            await _dbContext.UsersToGroups.AddAsync(userToGroup);
            await _dbContext.SaveChangesAsync();

            var sut = new GetMyGroupsHandler(validator, _dbContext, _groupsConfig);

            // Act
            var result = await sut.Handle(new GetMyGroups(userSub), CancellationToken.None);

            // Assert
            result.Value.Where(g => g.Id.Equals(group.Id)).First().MaxNumberOfMembers.Should().Be(35);
        }
    }
}