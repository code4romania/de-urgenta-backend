using System;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using Shouldly;
using Xunit;

namespace DeUrgenta.Certifications.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class DeleteCertificationValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public DeleteCertificationValidatorShould(DatabaseFixture fixture)
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
            var sut = new DeleteCertificationValidator(_dbContext);

            // Act
            var isValid = await sut.IsValidAsync(new DeleteCertification(sub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_certification_not_found()
        {
            // Arrange
            var sut = new DeleteCertificationValidator(_dbContext);
            var userSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new DeleteCertification(userSub, Guid.NewGuid()));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_owner()
        {
            // Arrange
            var sut = new DeleteCertificationValidator(_dbContext);

            var certificationId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var ownerSub = Guid.NewGuid().ToString();
            var userSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithId(ownerId).WithSub(ownerSub).Build();

            var certification = new Certification
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
            var isValid = await sut.IsValidAsync(new DeleteCertification(userSub, certificationId));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_request_when_user_is_owner()
        {
            // Arrange
            var sut = new DeleteCertificationValidator(_dbContext);

            var certificationId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var ownerSub = Guid.NewGuid().ToString();

            var owner = new UserBuilder().WithId(ownerId).WithSub(ownerSub).Build();

            var certification = new Certification
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
            var isValid = await sut.IsValidAsync(new DeleteCertification(ownerSub, certificationId));

            // Assert
            isValid.ShouldBeTrue();
        }

    }
}
