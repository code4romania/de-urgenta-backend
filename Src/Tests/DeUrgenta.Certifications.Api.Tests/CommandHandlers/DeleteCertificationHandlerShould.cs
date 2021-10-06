using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.CommandHandlers;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Certifications.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class DeleteCertificationHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;

        public DeleteCertificationHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<DeleteCertification>>();
            validator
                .IsValidAsync(Arg.Any<DeleteCertification>())
                .Returns(Task.FromResult(false));

            var sut = new DeleteCertificationHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new DeleteCertification("a-sub", Guid.NewGuid()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}
