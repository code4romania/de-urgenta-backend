using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Certifications.Api.QueryHandlers;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Tests.Helpers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace DeUrgenta.Certifications.Api.Tests.QueryHandlers
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetCertificationsHandlerShould
    {
        private readonly DeUrgentaContext _dbContext;
        public GetCertificationsHandlerShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<GetCertifications>>();
            validator
                .IsValidAsync(Arg.Any<GetCertifications>()) 
                .Returns(Task.FromResult(false));

            var sut = new GetCertificationsHandler(validator, _dbContext);

            // Act
            var result = await sut.Handle(new GetCertifications("a-sub"), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }
    }
}
