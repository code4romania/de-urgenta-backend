using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.QueryHandlers;
using DeUrgenta.Group.Api.Validators;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.QueriesHandlers
{
    [Collection("Database collection")]
    public class GetGroupMembersHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetGroupMembersHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetGroupMembers>>();
            validator
                .IsValidAsync(Arg.Any<GetGroupMembers>())
                .Returns(Task.FromResult(false));

            var sut = new GetGroupMembersHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetGroupMembers("a-sub", Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}