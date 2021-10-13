using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Invite.Api.CommandHandlers;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class CreateInviteHandlerShould
    {
        private readonly DeUrgentaContext _context;

        public CreateInviteHandlerShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            //Arrange
            var userSub = Guid.NewGuid().ToString();
            var request = new CreateInvite(userSub, new InviteRequest());

            var validator = Substitute.For<IValidateRequest<CreateInvite>>();
            validator.IsValidAsync(request).Returns(ValidationResult.GenericValidationError);
            var sut = new CreateInviteHandler(_context, validator);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
