using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.CommandHandlers;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Options;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateBackpackHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IOptions<BackpacksConfig> _config;

        public UpdateBackpackHandlerShould(DatabaseFixture fixture)
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
            var validator = Substitute.For<IValidateRequest<UpdateBackpack>>();
            validator
                .IsValidAsync(Arg.Any<UpdateBackpack>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new UpdateBackpackHandler(validator, _dbContext, _config);

            // Act
            var result = await sut.Handle(new UpdateBackpack("a-sub", Guid.NewGuid(), new BackpackModelRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}