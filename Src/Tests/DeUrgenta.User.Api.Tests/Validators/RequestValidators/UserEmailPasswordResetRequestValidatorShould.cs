using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Validators.RequestValidators;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators.RequestValidators
{
    public class UserEmailPasswordResetRequestValidatorShould
    {
        private readonly UserEmailPasswordResetRequestValidator _sut = new();

        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Invalidate_request_when_email_is_empty(string emptyEmail)
        {
            //Arrange
            var request = new UserEmailPasswordResetRequest
            {
                Email = emptyEmail
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.Email)
                .WithErrorMessage("'Email' must not be empty.");
        }

        [Theory]
        [InlineData("a.a")]
        [InlineData("aa")]
        public void Invalidate_request_when_email_is_not_email_format(string invalidEmail)
        {
            //Arrange
            var request = new UserEmailPasswordResetRequest
            {
                Email = invalidEmail
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.Email)
                .WithErrorMessage("'Email' is not a valid email address.");
        }

        [Fact]
        public void Validate_request_when_all_fields_are_valid()
        {
            //Arrange
            var request = new UserEmailPasswordResetRequest
            {
                Email = "a@a.com"
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
