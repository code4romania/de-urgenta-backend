using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Validators.RequestValidators;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.Group.Api.Tests.Validators.RequestValidators
{
    public class SafeLocationRequestValidatorShould
    {
        private readonly SafeLocationRequestValidator _sut = new();

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_name_is_empty(string emptyName)
        {
            //Arrange
            var request = new SafeLocationRequest
            {
                Name = emptyName
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("'Name' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_name_length_is_less_than_3_characters()
        {
            //Arrange
            var request = new SafeLocationRequest
            {
                Name = TestDataProviders.RandomString(1)
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("The length of 'Name' must be at least 3 characters. You entered 1 characters.");
        }

        [Fact]
        public void Invalidate_request_when_name_length_is_more_than_3_characters()
        {
            //Arrange
            var request = new SafeLocationRequest
            {
                Name = TestDataProviders.RandomString(101)
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("The length of 'Name' must be 100 characters or fewer. You entered 101 characters.");
        }

        [Fact]
        public void Validate_request_when_all_fields_are_valid()
        {
            //Arrange 
            var request = new SafeLocationRequest
            {
                Name = TestDataProviders.RandomString(4)
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
