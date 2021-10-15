using System;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Validators.RequestValidators;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Tests.Helpers;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.Backpack.Api.Tests.Validators.RequestValidators
{
    public class BackpackItemRequestValidatorShould
    {
        private readonly BackpackItemRequestValidator _sut =  new();
        
        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Invalidate_request_when_name_is_empty(string emptyName)
        {
            //Arrange 
            var backpackItemRequest = new BackpackItemRequest
            {
                Name = emptyName
            };

            //Act
            var result = _sut.TestValidate(backpackItemRequest);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("'Name' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_name_length_is_shorter_than_3_characters()
        {
            //Arrange 
            var backpackItemRequest = new BackpackItemRequest
            {
                Name = TestDataProviders.RandomString(1)
            };

            //Act
            var result = _sut.TestValidate(backpackItemRequest);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("The length of 'Name' must be at least 3 characters. You entered 1 characters.");
        }

        [Fact]
        public void Invalidate_request_when_name_length_is_longer_than_250_characters()
        {
            //Arrange 
            var backpackItemRequest = new BackpackItemRequest
            {
                Name = TestDataProviders.RandomString(251)
            };

            //Act
            var result = _sut.TestValidate(backpackItemRequest);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("The length of 'Name' must be 250 characters or fewer. You entered 251 characters.");
        }

        [Fact]
        public void Invalidate_request_when_amount_is_less_than_1()
        {
            var backpackItemRequest = new BackpackItemRequest
            {
                Amount = 0
            };

            //Act
            var result = _sut.TestValidate(backpackItemRequest);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Amount)
                .WithErrorMessage("'Amount' must be between 1 and 99999. You entered 0.");
        }

        [Fact]
        public void Invalidate_request_when_amount_is_more_than_99999()
        {
            var backpackItemRequest = new BackpackItemRequest
            {
                Amount = 100000
            };

            //Act
            var result = _sut.TestValidate(backpackItemRequest);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Amount)
                .WithErrorMessage("'Amount' must be between 1 and 99999. You entered 100000.");
        }

        [Fact]
        public void Invalidate_request_when_expiration_date_is_in_the_past()
        {
            var backpackItemRequest = new BackpackItemRequest
            {
                ExpirationDate = DateTime.Today.AddDays(-1)
            };

            //Act
            var result = _sut.TestValidate(backpackItemRequest);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.ExpirationDate)
                .WithErrorMessage($"'Expiration Date' must be greater than or equal to '{DateTime.Today}'.");
        }

        [Fact]
        public void Invalidate_request_when_category_type_is_not_specified()
        {
            var backpackItemRequest = new BackpackItemRequest
            {
                CategoryType = 0
            };

            //Act
            var result = _sut.TestValidate(backpackItemRequest);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.CategoryType)
                .WithErrorMessage("'Category Type' has a range of values which does not include '0'.");
        }

        [Fact]
        public void Validate_request_when_all_fields_are_valid()
        {
            var backpackItemRequest = new BackpackItemRequest
            {
                Name = "abcd",
                Amount = 1,
                ExpirationDate = DateTime.Today.AddDays(1),
                CategoryType = BackpackCategoryType.FirstAid
            };

            //Act
            var result = _sut.TestValidate(backpackItemRequest);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
