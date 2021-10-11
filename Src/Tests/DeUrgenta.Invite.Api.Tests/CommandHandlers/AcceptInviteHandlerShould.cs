using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Invite.Api.CommandHandlers;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class AcceptInviteHandlerShould
    {
        private readonly DeUrgentaContext _context;
        private readonly IServiceProvider _serviceProvider;

        public AcceptInviteHandlerShould(DatabaseFixture fixture)
        {
            _context = fixture.Context;

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();

            _serviceProvider = new ServiceCollection()
                .AddSingleton(x => _context)
                .AddInviteApiServices(configuration)
                .BuildServiceProvider();
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
            var sut = new AcceptInviteHandler(_context, validator, new InviteHandlerFactory(_serviceProvider));

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
