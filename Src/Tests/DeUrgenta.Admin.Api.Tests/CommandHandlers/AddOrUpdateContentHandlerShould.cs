using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.CommandHandlers;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.I18n.Service.Providers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.CommandHandlers
{
    public class AddOrUpdateContentHandlerShould
    {
        [Fact]
        public async Task Return_failed_result_when_validation_fails()
        {
            // Arrange
            var validator = Substitute.For<IValidateRequest<AddOrUpdateContent>>();
            validator
                .IsValidAsync(Arg.Any<AddOrUpdateContent>())
                .Returns(Task.FromResult(ValidationResult.GenericValidationError));

            var sut = new AddOrUpdateContentHandler(Substitute.For<IAmContentProvider>(), validator);

            // Act
            var result = await sut.Handle(new AddOrUpdateContent("culture", "key", "value"), CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
        }
    }
}