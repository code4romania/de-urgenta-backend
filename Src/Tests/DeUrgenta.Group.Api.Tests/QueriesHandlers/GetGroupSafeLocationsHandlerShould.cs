using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.QueryHandlers;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.QueriesHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetGroupSafeLocationsHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetGroupSafeLocationsHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetGroupSafeLocations>>();
            validator
                .IsValidAsync(Arg.Any<GetGroupSafeLocations>(), CancellationToken.None)
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new GetGroupSafeLocationsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetGroupSafeLocations("a-sub", Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}