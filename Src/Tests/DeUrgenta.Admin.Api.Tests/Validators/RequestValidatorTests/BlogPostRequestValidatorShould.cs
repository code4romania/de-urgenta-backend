using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Validators.RequestValidators;
using DeUrgenta.Tests.Helpers;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.Validators.RequestValidatorTests
{
    public class BlogPostRequestValidatorShould
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_title_is_empty(string invalidTitle)
        {
            // Arrange
            var request = new BlogPostRequest
            {
                Title = invalidTitle
            };
            var sut = new BlogPostRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Title)
                .WithErrorMessage("'Title' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_title_is_shorter_than_3_characters()
        {
            // Arrange
            var request = new BlogPostRequest
            {
                Title = TestDataProviders.RandomString(1)
            };
            var sut = new BlogPostRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Title)
                .WithErrorMessage("The length of 'Title' must be at least 3 characters. You entered 1 characters.");
        }

        [Fact]
        public void Invalidate_request_when_title_is_longer_than_250_characters()
        {
            // Arrange
            var request = new BlogPostRequest
            {
                Title = TestDataProviders.RandomString(251)
            };
            var sut = new BlogPostRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Title)
                .WithErrorMessage("The length of 'Title' must be 250 characters or fewer. You entered 251 characters.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_author_is_empty(string invalidAuthor)
        {
            // Arrange
            var request = new BlogPostRequest
            {
                Author = invalidAuthor
            };
            var sut = new BlogPostRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Author)
                .WithErrorMessage("'Author' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_author_is_shorter_than_3_characters()
        {
            // Arrange
            var request = new BlogPostRequest
            {
                Author = TestDataProviders.RandomString(1)
            };
            var sut = new BlogPostRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Author)
                .WithErrorMessage("The length of 'Author' must be at least 3 characters. You entered 1 characters.");
        }

        [Fact]
        public void Invalidate_request_when_author_is_longer_than_250_characters()
        {
            // Arrange
            var request = new BlogPostRequest
            {
                Author = TestDataProviders.RandomString(101)
            };
            var sut = new BlogPostRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Author)
                .WithErrorMessage("The length of 'Author' must be 100 characters or fewer. You entered 101 characters.");
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_contentBody_is_empty(string invalidContentBody)
        {
            // Arrange
            var request = new BlogPostRequest
            {
                ContentBody = invalidContentBody
            };
            var sut = new BlogPostRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ContentBody)
                .WithErrorMessage("'Content Body' must not be empty.");
        }
    }
}
