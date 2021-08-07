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
    public class CreateCertificationHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;
        public CreateCertificationHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<CreateCertification>>();
            validator
                .IsValidAsync(Arg.Any<CreateCertification>())
                .Returns(Task.FromResult(false));

            var sut = new CreateCertificationHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new CreateCertification("a-sub", new CertificationRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}
