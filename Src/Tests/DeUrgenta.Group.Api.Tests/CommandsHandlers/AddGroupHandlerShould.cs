using System.Threading;
using System.Threading.Tasks;
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
    public class AddGroupHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;
        
        public AddGroupHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<AddGroup>>();
            validator
                .IsValidAsync(Arg.Any<AddGroup>())
                .Returns(Task.FromResult(false));

            var sut = new AddGroupHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new AddGroup("a-sub", new GroupRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}