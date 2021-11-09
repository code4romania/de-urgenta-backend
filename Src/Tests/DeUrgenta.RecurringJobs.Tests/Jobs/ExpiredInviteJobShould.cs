using System;
using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.RecurringJobs.Jobs;
using DeUrgenta.RecurringJobs.Jobs.Config;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace DeUrgenta.RecurringJobs.Tests.Jobs
{
    [Collection(TestsConstants.DbCollectionName)]
    public class ExpiredInviteJobShould
    {
        private readonly DeUrgentaContext _dbContext;

        public ExpiredInviteJobShould(JobsDatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Remove_sent_invite_when_configured_period_has_passed()
        {
            //Arrange
            var invite = new InviteBuilder()
                .WithStatus(InviteStatus.Sent)
                .WithSentOn(DateTime.Today.AddDays(-11))
                .Build();
            await _dbContext.Invites.AddAsync(invite);
            await _dbContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredInviteJobConfig>>();
            jobConfig.Value.Returns(new ExpiredInviteJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredInviteJob(_dbContext, jobConfig);

            //Act
            await sut.RunAsync();

            //Assert
            var remainingInvite = await _dbContext.Invites.FirstOrDefaultAsync(i => i.Id == invite.Id);
            remainingInvite.Should().BeNull();
        }

        [Fact]
        public async Task Does_not_remove_sent_invite_when_configured_period_has_not_passed()
        {
            //Arrange
            var invite = new InviteBuilder()
                .WithStatus(InviteStatus.Sent)
                .WithSentOn(DateTime.Today.AddDays(-9))
                .Build();
            await _dbContext.Invites.AddAsync(invite);
            await _dbContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredInviteJobConfig>>();
            jobConfig.Value.Returns(new ExpiredInviteJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredInviteJob(_dbContext, jobConfig);

            //Act
            await sut.RunAsync();

            //Assert
            var remainingInvite = await _dbContext.Invites.FirstOrDefaultAsync(i => i.Id == invite.Id);
            remainingInvite.Should().NotBeNull();
        }

        [Fact]
        public async Task Does_not_remove_invite_when_invite_is_not_in_sent_status()
        {
            //Arrange
            var invite = new InviteBuilder()
                .WithStatus(InviteStatus.Accepted)
                .WithSentOn(DateTime.Today.AddDays(-11))
                .Build();
            await _dbContext.Invites.AddAsync(invite);
            await _dbContext.SaveChangesAsync();

            var jobConfig = Substitute.For<IOptions<ExpiredInviteJobConfig>>();
            jobConfig.Value.Returns(new ExpiredInviteJobConfig
            {
                DaysBeforeExpirationDate = 10
            });
            var sut = new ExpiredInviteJob(_dbContext, jobConfig);

            //Act
            await sut.RunAsync();

            //Assert
            var remainingInvite = await _dbContext.Invites.FirstOrDefaultAsync(i => i.Id == invite.Id);
            remainingInvite.Should().NotBeNull();
        }
    }
}
