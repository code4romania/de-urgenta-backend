using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.CommandHandlers;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Storage;
using DeUrgenta.Certifications.Api.Tests.Builders;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
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
            var storage = Substitute.For<IBlobStorage>();
            var validator = Substitute.For<IValidateRequest<UpdateCertification>>();
            validator
                .IsValidAsync(Arg.Any<UpdateCertification>())
                .Returns(Task.FromResult(false));

            var sut = new UpdateCertificationHandler(validator, _dbContext, storage);

            // Act
            var result = await sut.Handle(new UpdateCertification("a-sub", Guid.NewGuid(), new CertificationRequest()), CancellationToken.None);

            // Assert
            result.IsFailure.ShouldBeTrue();
        }

        [Fact]
        public async Task Return_success_if_photo_is_not_provided_in_request()
        {
            //Arrange
            var certificationRequest = new CertificationRequestBuilder().WithPhoto(null).Build();

            var certificationId = Guid.NewGuid();
            var updateCertification = new UpdateCertification(TestDataProviders.RandomString(), certificationId, certificationRequest);

            var storage = Substitute.For<IBlobStorage>();
            var validator = Substitute.For<IValidateRequest<UpdateCertification>>();
            validator
                .IsValidAsync(updateCertification)
                .Returns(Task.FromResult(true));
            await SetupExistingCertification(certificationId);

            var sut = new UpdateCertificationHandler(validator, _dbContext, storage);

            //Act
            var result = await sut.Handle(updateCertification, CancellationToken.None);

            //Assert
            result.IsSuccess.ShouldBeTrue();
        }

        private async Task SetupExistingCertification(Guid certificationId)
        {
            var userId = Guid.NewGuid();
            var user = new UserBuilder().WithId(userId).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Certifications.AddAsync(new Certification
            {
                Name = TestDataProviders.RandomString(),
                ExpirationDate = DateTime.Today,
                IssuingAuthority = TestDataProviders.RandomString(),
                Id = certificationId,
                UserId = userId
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}
