using DeUrgenta.Tests.Helpers;
using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Validators.RequestValidators;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.User.Api.Tests.Validators.RequestValidators
{
    public class UserResetPasswordRequestValidatorShould
    {
        private readonly UserResetPasswordRequestValidator _sut = new();

        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Invalidate_request_when_first_name_is_empty(string emptyUserId)
        {
            //Arrange
            var request = new UserResetPasswordRequest
            {
                UserId = emptyUserId
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.UserId)
                .WithErrorMessage("'User Id' must not be empty.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Invalidate_request_when_reset_token_is_empty(string emptyResetToken)
        {
            //Arrange
            var request = new UserResetPasswordRequest
            {
                ResetToken = emptyResetToken
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.ResetToken)
                .WithErrorMessage("'Reset Token' must not be empty.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Invalidate_request_when_new_password_is_empty(string emptyNewPassword)
        {
            //Arrange
            var request = new UserResetPasswordRequest
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
        public void Invalidate_request_when_new_password_confirm_is_not_equal_to_new_password()
        {
            //Arrange
            var newPassword = TestDataProviders.RandomString();
            var request = new UserResetPasswordRequest
            {
                NewPassword = newPassword,
                NewPasswordConfirm = TestDataProviders.RandomString()
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldHaveValidationErrorFor(f => f.NewPasswordConfirm)
                .WithErrorMessage($"'New Password Confirm' must be equal to '{newPassword}'.");
        }

        [Fact]
        public void Validate_request_when_all_fields_are_valid()
        {
            //Arrange
            var newPassword = TestDataProviders.RandomString();
            var request = new UserResetPasswordRequest
            {
                UserId = TestDataProviders.RandomString(),
                ResetToken = TestDataProviders.RandomString(),
                NewPassword = newPassword,
                NewPasswordConfirm = newPassword
            };

            //Act
            var result = _sut.TestValidate(request);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
