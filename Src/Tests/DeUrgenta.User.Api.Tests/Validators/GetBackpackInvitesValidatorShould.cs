﻿using System;
using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using DeUrgenta.User.Api.Queries;
using DeUrgenta.User.Api.Validators;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetBackpackInvitesValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetBackpackInvitesValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("my-weird-sub")]
        public async Task Invalidate_request_when_no_user_found_by_sub(string sub)
        {
            // Arrange
            var sut = new GetBackpackInvitesValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new GetBackpackInvites(sub));

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public async Task Validate_when_user_was_found_by_sub()
        {
            var sut = new GetBackpackInvitesValidator(_dbContext);

            // Arrange
            var userSub = Guid.NewGuid().ToString();
            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new GetBackpackInvites(userSub));

            // Assert
            isValid.Should().BeTrue();
        }
    }
}