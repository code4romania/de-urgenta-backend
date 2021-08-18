using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.QueryHandlers;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.QueriesHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetMyGroupsHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetMyGroupsHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetMyGroups>>();
            validator
                .IsValidAsync(Arg.Any<GetMyGroups>())
                .Returns(Task.FromResult(false));

            var sut = new GetMyGroupsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetMyGroups("a-sub"), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}