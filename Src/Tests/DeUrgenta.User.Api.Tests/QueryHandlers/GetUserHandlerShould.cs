using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.User.Api.Queries;
using DeUrgenta.User.Api.QueryHandlers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.User.Api.Tests.QueryHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetUserHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetUserHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetUser>>();
            validator
                .IsValidAsync(Arg.Any<GetUser>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new GetUserHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetUser("a-sub"), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}