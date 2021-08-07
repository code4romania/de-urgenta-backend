using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.CommandHandlers;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Certifications.Api.Tests.CommandHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateCertificationHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;
        public UpdateCertificationHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<UpdateCertification>>();
            validator
                .IsValidAsync(Arg.Any<UpdateCertification>())
                .Returns(Task.FromResult(false));

            var sut = new UpdateCertificationHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new UpdateCertification("a-sub", Guid.NewGuid(), new CertificationRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }

    }
}
