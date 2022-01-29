using System;
using DeUrgenta.Invite.Api.Models;
using DeUrgenta.Invite.Api.Validators.RequestValidators;
using FluentValidation.TestHelper;
using Xunit;

namespace DeUrgenta.Invite.Api.Tests.Validators.RequestValidators
{
    public class InviteRequestValidatorShould
    {
        [Fact]
        public void Invalidate_request_when_destination_id_is_empty()
        {
            // Arrange
            var inviteRequest = new InviteRequest
            {
                DestinationId = Guid.Empty
            };
            var sut = new InviteRequestValidator();

            // Act
            var result = sut.TestValidate(inviteRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.DestinationId)
                .WithErrorMessage("'Destination Id' must not be empty.");
        }


        [Fact]
        public void Invalidate_request_when_type_is_not_known()
        {
            // Arrange
            var inviteRequest = new InviteRequest
            {
                Type = (InviteType)3
            };
            var sut = new InviteRequestValidator();

            // Act
            var result = sut.TestValidate(inviteRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Type)
                .WithErrorMessage("'Type' has a range of values which does not include '3'.");
        }

        [Fact]
        public void Validate_request_when_all_fields_are_valid()
        {
            var inviteRequest = new InviteRequest
            {
                DestinationId = Guid.NewGuid(),
                Type = InviteType.Backpack
            };
            var sut = new InviteRequestValidator();

            // Act
            var result = sut.TestValidate(inviteRequest);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
