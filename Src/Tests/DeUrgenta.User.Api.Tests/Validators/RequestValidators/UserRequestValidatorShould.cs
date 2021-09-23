using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Validators.RequestValidators;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators.RequestValidators
{
    public class UserRequestValidatorShould
    {
        private readonly UserRequestValidator _sut = new();

        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Invalidate_request_when_first_name_is_empty(string emptyFirstName)
        {
            //Arrange
            var request = new UserRequest { FirstName = emptyFirstName };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.FirstName)
                .WithErrorMessage("'First Name' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_first_name_length_is_more_than_100()
        {
            //Arrange
            var request = new UserRequest
            {
                FirstName = TestDataProviders.RandomString(101)
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.FirstName)
                .WithErrorMessage("The length of 'First Name' must be 100 characters or fewer. You entered 101 characters.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Invalidate_request_when_last_name_is_empty(string emptyFirstName)
        {
            //Arrange
            var request = new UserRequest
            {
                LastName = emptyFirstName
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.LastName)
                .WithErrorMessage("'Last Name' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_last_name_length_is_more_than_100()
        {
            //Arrange
            var request = new UserRequest
            {
                LastName = TestDataProviders.RandomString(101)
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.LastName)
                .WithErrorMessage("The length of 'Last Name' must be 100 characters or fewer. You entered 101 characters.");
        }

        [Fact]
        public void Validate_request_when_all_fields_are_valid()
        {
            //Arrange
            var request = new UserRequest
            {
                FirstName = TestDataProviders.RandomString(),
                LastName = TestDataProviders.RandomString()
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
