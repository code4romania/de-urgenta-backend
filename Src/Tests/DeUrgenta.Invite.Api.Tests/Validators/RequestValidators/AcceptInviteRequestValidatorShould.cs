using System;
using DeUrgenta.Invite.Api.Models;
using DeUrgenta.Invite.Api.Validators.RequestValidators;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.Validators.RequestValidators
{
    public class AcceptInviteRequestValidatorShould
    {
        [Fact]
        public void Invalidate_request_when_invite_id_is_empty()
        {
            // Arrange
            var acceptInviteRequest = new AcceptInviteRequest
            {
                InviteId = Guid.Empty
            };
            var sut = new AcceptInviteRequestValidator();

            // Act
            var result = sut.TestValidate(acceptInviteRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.InviteId)
                .WithErrorMessage("'Invite Id' must not be empty.");
        }

        [Fact]
        public void Validate_request_when_all_fields_are_valid()
        {
            var acceptInviteRequest = new AcceptInviteRequest
            {
                InviteId = Guid.NewGuid()
            };
            var sut = new AcceptInviteRequestValidator();

            // Act
            var result = sut.TestValidate(acceptInviteRequest);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
