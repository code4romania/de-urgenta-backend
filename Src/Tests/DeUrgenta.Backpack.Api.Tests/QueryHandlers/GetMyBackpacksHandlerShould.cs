using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Options;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.QueryHandlers;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.QueryHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetMyBackpacksHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IOptions<BackpacksConfig> _config;

        public GetMyBackpacksHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            var options = new BackpacksConfig
            {
                MaxContributors = 2
            };
            _config = Microsoft.Extensions.Options.Options.Create(options);
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetMyBackpacks>>();
            validator
                .IsValidAsync(Arg.Any<GetMyBackpacks>(), CancellationToken.None)
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new GetMyBackpacksHandler(validator, _dbContext, _config);

            // Act
            var result = await sut.Handle(new GetMyBackpacks("a-sub"), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}