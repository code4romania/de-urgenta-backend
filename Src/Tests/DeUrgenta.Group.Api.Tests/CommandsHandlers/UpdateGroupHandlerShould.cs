using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.CommandHandlers;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Validators;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.CommandsHandlers
{
    [Collection("Database collection")]
    public class UpdateGroupHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateGroupHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<UpdateGroup>>();
            validator
                .IsValidAsync(Arg.Any<UpdateGroup>())
                .Returns(Task.FromResult(false));

            var sut = new UpdateGroupHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new UpdateGroup("a-sub", Guid.NewGuid(), new GroupRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}