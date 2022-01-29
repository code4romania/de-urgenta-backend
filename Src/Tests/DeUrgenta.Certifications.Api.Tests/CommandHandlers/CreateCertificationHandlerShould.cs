using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.CommandHandlers;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Storage;
using DeUrgenta.Certifications.Api.Tests.Builders;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using NSubstitute;
using FluentAssertions;
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
            var storage = Substitute.For<IBlobStorage>();
            var validator = Substitute.For<IValidateRequest<CreateCertification>>();
            validator
                .IsValidAsync(Arg.Any<CreateCertification>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new CreateCertificationHandler(validator, _dbContext, storage);

            // Act
            var result = await sut.Handle(new CreateCertification("a-sub", new CertificationRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Return_success_if_photo_is_not_provided_in_request()
        {
            //Arrange
            var userSub = TestDataProviders.RandomString();
            var certificationRequest = new CertificationRequestBuilder()
                .WithPhoto(null)
                .Build();

            var user = new UserBuilder().WithSub(userSub).Build();
            
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var createCertification = new CreateCertification(userSub, certificationRequest);

            var storage = Substitute.For<IBlobStorage>();
            var validator = Substitute.For<IValidateRequest<CreateCertification>>();
            validator
                .IsValidAsync(createCertification)
                .Returns(Task.FromResult(ValidationResult.Ok));

            var sut = new CreateCertificationHandler(validator, _dbContext, storage);

            //Act
            var result = await sut.Handle(createCertification, CancellationToken.None);

            //Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}
