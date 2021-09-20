using System;
using System.IO;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Validators.RequestValidators;
using DeUrgenta.Tests.Helpers;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace DeUrgenta.Certifications.Api.Tests.Validators.RequestValidatorTests
{
    public class CertificationRequestValidatorShould
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_name_is_empty(string invalidName)
        {
            // Arrange
            var certificationRequest = new CertificationRequest
            {
                Name = invalidName
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("'Name' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_name_is_shorter_than_3_characters()
        {
            // Arrange
            var certificationRequest = new CertificationRequest
            {
                Name = TestDataProviders.RandomString(1)
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("The length of 'Name' must be at least 3 characters. You entered 1 characters.");
        }

        [Fact]
        public void Invalidate_request_when_name_is_longer_than_250_characters()
        {
            // Arrange
            var certificationRequest = new CertificationRequest
            {
                Name = TestDataProviders.RandomString(251)
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("The length of 'Name' must be 250 characters or fewer. You entered 251 characters.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_issuing_authority_is_empty(string invalidIssuingAuthority)
        {
            // Arrange
            var certificationRequest = new CertificationRequest
            {
                IssuingAuthority = invalidIssuingAuthority
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.IssuingAuthority)
                .WithErrorMessage("'Issuing Authority' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_issuing_authority_is_shorter_than_3_characters()
        {
            // Arrange
            var certificationRequest = new CertificationRequest
            {
                IssuingAuthority = TestDataProviders.RandomString(1)
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.IssuingAuthority)
                .WithErrorMessage("The length of 'Issuing Authority' must be at least 3 characters. You entered 1 characters.");
        }

        [Fact]
        public void Invalidate_request_when_issuing_authority_is_longer_than_250_characters()
        {
            // Arrange
            var certificationRequest = new CertificationRequest
            {
                IssuingAuthority = TestDataProviders.RandomString(251)
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.IssuingAuthority)
                .WithErrorMessage("The length of 'Issuing Authority' must be 250 characters or fewer. You entered 251 characters.");
        }

        [Fact]
        public void Invalidate_request_when_expiration_date_is_in_the_past()
        {
            // Arrange
            var certificationRequest = new CertificationRequest
            {
                ExpirationDate = DateTime.Today.AddDays(-1)
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ExpirationDate)
                .WithErrorMessage($"'Expiration Date' must be greater than or equal to '{DateTime.Today}'.");
        }

        [Fact]
        public void Invalidate_request_when_photo_name_is_longer_than_250_characters()
        {
            // Arrange
            var fileName = TestDataProviders.RandomString(251);
            var formFile = new FormFile(new MemoryStream(), 0, int.MaxValue, "", fileName)
            {
                Headers = new HeaderDictionary()
            };
            formFile.Headers.Add(HeaderNames.ContentType, "image/png");

            var certificationRequest = new CertificationRequest
            {
                Photo = formFile
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Photo.FileName)
                .WithErrorMessage("File name must not be longer than 250 characters.");
        }

        [Fact]
        public void Invalidate_request_when_photo_id_larger_than_5_megabytes()
        {
            // Arrange
            int fileLength = int.MaxValue;
            var formFile = new FormFile(new MemoryStream(), 0, fileLength, "", TestDataProviders.RandomString())
            {
                Headers = new HeaderDictionary()
            };
            formFile.Headers.Add(HeaderNames.ContentType, "image/png");

            var certificationRequest = new CertificationRequest
            {
                Photo = formFile
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Photo.Length)
                .WithErrorMessage("File size is larger than 5 MB.");
        }

        [Fact]
        public void Invalidate_request_when_photo_has_incorrect_content_type()
        {
            // Arrange
            var formFile = new FormFile(new MemoryStream(), 0, 5000, "", TestDataProviders.RandomString())
            {
                Headers = new HeaderDictionary()
            };
            formFile.Headers.Add(HeaderNames.ContentType, "application/json");

            var certificationRequest = new CertificationRequest
            {
                Photo = formFile
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Photo.ContentType)
                .WithErrorMessage("File must be an image.");
        }

        [Fact]
        public void Validate_request_when_all_fields_valid()
        {
            //Arrange
            var formFile = new FormFile(new MemoryStream(), 0, 5000, "", TestDataProviders.RandomString())
            {
                Headers = new HeaderDictionary()
            };
            formFile.Headers.Add(HeaderNames.ContentType, "image/jpeg");

            var certificationRequest = new CertificationRequest
            {
                Name = TestDataProviders.RandomString(),
                ExpirationDate = DateTime.Today.AddDays(1),
                IssuingAuthority = TestDataProviders.RandomString(),
                Photo = formFile
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_request_when_photo_not_provided()
        {
            //Arrange
            var certificationRequest = new CertificationRequest
            {
                Name = TestDataProviders.RandomString(),
                ExpirationDate = DateTime.Today.AddDays(1),
                IssuingAuthority = TestDataProviders.RandomString()
            };
            var sut = new CertificationRequestValidator();

            // Act
            var result = sut.TestValidate(certificationRequest);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
