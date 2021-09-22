using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Validators.RequestValidators;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators.RequestValidators
{
    public class UserSafeLocationRequestValidatorShould
    {
        private readonly UserSafeLocationRequestValidator _sut = new();

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_address_is_empty(string address)
        {
            //Arrange
            var request = new UserLocationRequest
            {
                Address = address
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Address)
                .WithErrorMessage("'Address' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_address_length_is_less_than_3_characters()
        {
            //Arrange
            var request = new UserLocationRequest
            {
                Address = TestDataProviders.RandomString(1)
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Address)
                .WithErrorMessage("The length of 'Address' must be at least 3 characters. You entered 1 characters.");
        }

        [Fact]
        public void Invalidate_request_when_address_length_is_more_than_3_characters()
        {
            //Arrange
            var request = new UserLocationRequest
            {
                Address = TestDataProviders.RandomString(101)
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(c => c.Address)
                .WithErrorMessage("The length of 'Address' must be 100 characters or fewer. You entered 101 characters.");
        }

        [Fact]
        public void Validate_request_when_all_fields_are_valid()
        {
            //Arrange 
            var request = new UserLocationRequest
            {
                Address = TestDataProviders.RandomString(4)
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
