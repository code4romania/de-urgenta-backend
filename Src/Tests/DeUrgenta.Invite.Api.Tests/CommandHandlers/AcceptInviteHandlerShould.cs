using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Invite.Api.CommandHandlers;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AcceptInviteHandlerShould
    {
        private readonly DeUrgentaContext _context;

        public AcceptInviteHandlerShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;

        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            //Arrange
            var userSub = Guid.NewGuid().ToString();
            var inviteId = Guid.NewGuid();
            var request = new AcceptInvite(userSub, inviteId);

            var validator = Substitute.For<IValidateRequest<AcceptInvite>>();
            validator.IsValidAsync(request).Returns(false);
            var mediator = Substitute.For<IMediator>();
            var sut = new AcceptInviteHandler(_context, validator, mediator);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
