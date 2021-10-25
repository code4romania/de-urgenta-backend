using System;
using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Tests.Helpers;
using DeUrgenta.Tests.Helpers.Builders;
using FluentAssertions;
using NSubstitute;
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
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sut = new UpdateCertificationValidator(_dbContext, i18nProvider);

            // Act
            var isValid = await sut.IsValidAsync(new UpdateCertification(sub, Guid.NewGuid(), new CertificationRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_certification_not_found()
        {
            // Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sut = new UpdateCertificationValidator(_dbContext, i18nProvider);
            var userSub = Guid.NewGuid().ToString();

            var user = new UserBuilder().WithSub(userSub).Build();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var isValid = await sut.IsValidAsync(new UpdateCertification(userSub, Guid.NewGuid(), new CertificationRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Invalidate_request_when_user_is_not_owner()
        {
            // Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sut = new UpdateCertificationValidator(_dbContext, i18nProvider);

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
            var isValid = await sut.IsValidAsync(new UpdateCertification(userSub, certificationId, new CertificationRequest()));

            // Assert
            isValid.Should().BeOfType<GenericValidationError>();
        }

        [Fact]
        public async Task Validate_request_when_user_is_owner()
        {
            // Arrange
            var i18nProvider = Substitute.For<IamI18nProvider>();
            i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");

            var sut = new UpdateCertificationValidator(_dbContext, i18nProvider);

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
            var isValid = await sut.IsValidAsync(new UpdateCertification(ownerSub, certificationId, new CertificationRequest()));

            // Assert
            isValid.Should().BeOfType<ValidationPassed>();
        }

    }
}
