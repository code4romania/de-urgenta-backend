using System;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Validators.RequestValidators;
using DeUrgenta.Tests.Helpers;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.Validators.RequestValidatorTests
{
    public class EventRequestValidatorShould
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_title_is_empty(string invalidTitle)
        {
            // Arrange
            var request = new EventRequest
            {
                Title = invalidTitle
            };
            var sut = new EventRequestValidator();

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
            var request = new EventRequest
            {
                Title = TestDataProviders.RandomString(1)
            };
            var sut = new EventRequestValidator();

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
            var request = new EventRequest
            {
                Title = TestDataProviders.RandomString(251)
            };
            var sut = new EventRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Title)
                .WithErrorMessage("The length of 'Title' must be 250 characters or fewer. You entered 251 characters.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_organizedBy_is_empty(string invalidOrganizedBy)
        {
            // Arrange
            var request = new EventRequest
            {
                OrganizedBy = invalidOrganizedBy
            };
            var sut = new EventRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.OrganizedBy)
                .WithErrorMessage("'Organized By' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_organizedBy_is_shorter_than_3_characters()
        {
            // Arrange
            var request = new EventRequest
            {
                OrganizedBy = TestDataProviders.RandomString(1)
            };
            var sut = new EventRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.OrganizedBy)
                .WithErrorMessage("The length of 'Organized By' must be at least 3 characters. You entered 1 characters.");
        }

        [Fact]
        public void Invalidate_request_when_organizedBy_is_longer_than_101_characters()
        {
            // Arrange
            var request = new EventRequest
            {
                OrganizedBy = TestDataProviders.RandomString(101)
            };
            var sut = new EventRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.OrganizedBy)
                .WithErrorMessage("The length of 'Organized By' must be 100 characters or fewer. You entered 101 characters.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_author_is_empty(string invalidAuthor)
        {
            // Arrange
            var request = new EventRequest
            {
                Author = invalidAuthor
            };
            var sut = new EventRequestValidator();

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
            var request = new EventRequest
            {
                Author = TestDataProviders.RandomString(1)
            };
            var sut = new EventRequestValidator();

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
            var request = new EventRequest
            {
                Author = TestDataProviders.RandomString(101)
            };
            var sut = new EventRequestValidator();

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
            var request = new EventRequest
            {
                ContentBody = invalidContentBody
            };
            var sut = new EventRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ContentBody)
                .WithErrorMessage("'Content Body' must not be empty.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_address_is_empty(string invalidAddress)
        {
            // Arrange
            var request = new EventRequest
            {
                Address = invalidAddress
            };
            var sut = new EventRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Address)
                .WithErrorMessage("'Address' must not be empty.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_locality_is_empty(string invalidLocality)
        {
            // Arrange
            var request = new EventRequest
            {
                Locality = invalidLocality
            };
            var sut = new EventRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Locality)
                .WithErrorMessage("'Locality' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_occursOn_is_in_the_past()
        {
            // Arrange
            var request = new EventRequest
            {
                OccursOn = DateTime.Today.AddDays(-1)
            };
            var sut = new EventRequestValidator();

            // Act
            var result = sut.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.OccursOn)
                .WithErrorMessage($"'Occurs On' must be greater than or equal to '{DateTime.Today}'.");
        }
    }
}
