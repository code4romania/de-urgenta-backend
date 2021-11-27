using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Validators.RequestValidators;
using DeUrgenta.Tests.Helpers;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.Validators.RequestValidatorTests
{
    public class AddOrUpdateContentModelValidatorShould
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_culture_is_empty(string invalidCulture)
        {
            // Arrange
            var request = new AddOrUpdateContentModel
            {
                Culture = invalidCulture
            };
            var sut = new AddOrUpdateContentModelValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Culture)
                .WithErrorMessage("'Culture' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_culture_is_longer_than_250_characters()
        {
            // Arrange
            var request = new AddOrUpdateContentModel
            {
                Culture = TestDataProviders.RandomString(251)
            };
            var sut = new AddOrUpdateContentModelValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Culture)
                .WithErrorMessage("The length of 'Culture' must be 250 characters or fewer. You entered 251 characters.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_key_is_empty(string invalidAuthor)
        {
            // Arrange
            var request = new AddOrUpdateContentModel
            {
                Key = invalidAuthor
            };
            var sut = new AddOrUpdateContentModelValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Key)
                .WithErrorMessage("'Key' must not be empty.");
        }


        [Fact]
        public void Invalidate_request_when_key_is_longer_than_250_characters()
        {
            // Arrange
            var request = new AddOrUpdateContentModel
            {
                Key = TestDataProviders.RandomString(251)
            };
            var sut = new AddOrUpdateContentModelValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Key)
                .WithErrorMessage("The length of 'Key' must be 250 characters or fewer. You entered 251 characters.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_value_is_empty(string invalidContentBody)
        {
            // Arrange
            var request = new AddOrUpdateContentModel
            {
                Value = invalidContentBody
            };
            var sut = new AddOrUpdateContentModelValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Value)
                .WithErrorMessage("'Value' must not be empty.");
        }
    }
}
