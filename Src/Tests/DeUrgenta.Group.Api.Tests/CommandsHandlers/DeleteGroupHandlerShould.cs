﻿using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.CommandHandlers;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.CommandsHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class DeleteGroupHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public DeleteGroupHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<DeleteGroup>>();
            validator
                .IsValidAsync(Arg.Any<DeleteGroup>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new DeleteGroupHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new DeleteGroup("a-sub", Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}