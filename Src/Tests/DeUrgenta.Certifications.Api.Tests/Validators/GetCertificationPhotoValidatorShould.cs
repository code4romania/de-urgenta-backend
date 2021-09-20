using System;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Certifications.Api.Validators;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Tests.Helpers;
using Shouldly;
using Xunit;

namespace DeUrgenta.Certifications.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class GetCertificationPhotoValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public GetCertificationPhotoValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("my-weird-sub")]
        public async Task Invalidate_request_when_no_user_found_by_sub(string sub)
        {
            //Arrange
            var certificationId = Guid.NewGuid();
            var sut = new GetCertificationPhotoValidator(_dbContext);

            //Act
            bool isValid = await sut.IsValidAsync(new GetCertificationPhoto(sub, certificationId));

            //Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Invalidate_when_certification_belongs_to_different_user()
        {
            // Arrange
            var sut = new GetCertificationPhotoValidator(_dbContext);

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
            bool isValid = await sut.IsValidAsync(new GetCertificationPhoto(userSub, certificationId));

            // Assert
            isValid.ShouldBeFalse();
        }

        [Fact]
        public async Task Validate_request_when_certification_belongs_to_user()
        {
            // Arrange
            var sut = new GetCertificationPhotoValidator(_dbContext);

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
            bool isValid = await sut.IsValidAsync(new GetCertificationPhoto(ownerSub, certificationId));

            // Assert
            isValid.ShouldBeTrue();
        }
    }
}
