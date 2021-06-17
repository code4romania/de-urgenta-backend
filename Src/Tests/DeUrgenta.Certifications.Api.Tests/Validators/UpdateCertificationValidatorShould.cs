using System;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using Shouldly;
using Xunit;

namespace DeUrgenta.Certifications.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateCertificationValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateCertificationValidatorShould(DatabaseFixture fixture)
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
            var sut = new UpdateCertificationValidator(_dbContext);

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateCertification(sub, Guid.NewGuid(), new CertificationRequest()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_certification_not_found()
        {
            // Arrange
            var sut = new UpdateCertificationValidator(_dbContext);
            string userSub = Guid.NewGuid().ToString();

            var user = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = userSub
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateCertification(userSub, Guid.NewGuid(), new CertificationRequest()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_owner()
        {
            // Arrange
            var sut = new UpdateCertificationValidator(_dbContext);

            var certificationId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var ownerSub = Guid.NewGuid().ToString();
            var userSub = Guid.NewGuid().ToString();

            var owner = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = ownerSub,
                Id = ownerId
            };

            var certification = new Domain.Entities.Certification
            {
                Id = certificationId,
                Name = "my certification",
                User = owner,
                IssuingAuthority = "test authority"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Certifications.AddAsync(certification);
            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateCertification(userSub, certificationId, new CertificationRequest()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_request_when_user_is_owner()
        {
            // Arrange
            var sut = new UpdateCertificationValidator(_dbContext);

            var certificationId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var ownerSub = Guid.NewGuid().ToString();

            var owner = new User
            {
                FirstName = "Integration",
                LastName = "Test",
                Sub = ownerSub,
                Id = ownerId
            };

            var certification = new Domain.Entities.Certification
            {
                Id = certificationId,
                Name = "my certification",
                User = owner,
                IssuingAuthority = "test authority"
            };

            await _dbContext.Users.AddAsync(owner);
            await _dbContext.Certifications.AddAsync(certification);
            await _dbContext.SaveChangesAsync();

            // Act
            bool isValid = await sut.IsValidAsync(new UpdateCertification(ownerSub, certificationId, new CertificationRequest()));

            // Assert
            isValid.ShouldBeTrue();
        }

    }
}
