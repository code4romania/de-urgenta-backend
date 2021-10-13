using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.User.Api.CommandHandlers;
using DeUrgenta.User.Api.Commands;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.User.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateLocationHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateLocationHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        //todo test the actual sut in the class name
        //[Fact]
        //public async Task Return_failed_result_when_validation_fails()
        //{
        //    // Arrange
        //    var validator = Substitute.For<IValidateRequest<AcceptGroupInvite>>();
        //    validator
        //        .IsValidAsync(Arg.Any<AcceptGroupInvite>())
        //        .Returns(Task.FromResult(false));

        //    var sut = new AcceptGroupInviteHandler(validator, _dbContext);

        //    // Act
        //    var result = await sut.Handle(new AcceptGroupInvite("a-sub", Guid.NewGuid()), CancellationToken.None);

        //    // Assert
        //    result.IsFailure.Should().BeTrue();
        //}
    }
}