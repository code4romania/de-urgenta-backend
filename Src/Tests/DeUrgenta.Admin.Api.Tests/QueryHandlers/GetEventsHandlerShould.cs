using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Admin.Api.QueryHandlers;
using DeUrgenta.Common.Models.Pagination;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.QueryHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetEventsHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetEventsHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetEvents>>();
            validator
                .IsValidAsync(Arg.Any<GetEvents>(), CancellationToken.None)
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new GetEventsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetEvents(new PaginationQueryModel { PageNumber = 1, PageSize = 1 }), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

    }
}