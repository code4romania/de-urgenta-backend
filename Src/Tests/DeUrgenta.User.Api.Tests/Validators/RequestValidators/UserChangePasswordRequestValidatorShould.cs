using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Validators.RequestValidators;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators.RequestValidators
{
    public class UserChangePasswordRequestValidatorShould
    {
        private readonly UserChangePasswordRequestValidator _sut = new();

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_confirm_new_password_is_empty(string emptyChangeNewPassword)
        {
            //Arrange
            var request =  new UserChangePasswordRequest
            {
                ConfirmNewPassword = emptyChangeNewPassword
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.ConfirmNewPassword)
                .WithErrorMessage("'Confirm New Password' must not be empty.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Invalidate_request_when_new_password_is_empty(string emptyNewPassword)
        {
            //Arrange
            var request = new UserChangePasswordRequest
            {
                NewPassword = emptyNewPassword
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.NewPassword)
                .WithErrorMessage("'New Password' must not be empty.");
        }

        [Fact]
        public void Invalidate_request_when_new_password_is_different_to_confirm_new_password()
        {
            //Arrange
            var newPassword = TestDataProviders.RandomString();
            var request = new UserChangePasswordRequest
            {
                NewPassword = newPassword,
                ConfirmNewPassword = TestDataProviders.RandomString()
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.ConfirmNewPassword)
                .WithErrorMessage($"'Confirm New Password' must be equal to '{newPassword}'.");
        }

        [Fact]
        public void Validate_request_when_all_fields_are_valid()
        {
            //Arrange
            var newPassword = TestDataProviders.RandomString();
            var request = new UserChangePasswordRequest
            {
                NewPassword = newPassword,
                ConfirmNewPassword = newPassword
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
