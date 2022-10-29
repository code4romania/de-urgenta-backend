using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.I18n.Service.Models;
using DeUrgenta.I18n.Service.Providers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.Validators
{
    public class AddOrUpdateContentValidatorShould
    {
        [Fact]
        public async Task Invalidate_request_when_not_existing_language_culture()
        {
            // Arrange

            var languageProvider = Substitute.For<IAmLanguageProvider>();
            languageProvider
                .GetLanguageByCulture(Arg.Any<string>())
                .ReturnsForAnyArgs(Task.FromResult<LanguageModel>(null));

            var sut = new AddOrUpdateContentValidator(languageProvider);

            // Act
            var requestCulture = "culture";

            var result = await sut.IsValidAsync(new AddOrUpdateContent(requestCulture, "key", "value"), CancellationToken.None);

            // Assert
            result
                .Should()
                .BeOfType<LocalizableValidationError>()
                .Which.Messages
                .Should()
                .BeEquivalentTo(new Dictionary<LocalizableString, LocalizableString>
                {
                    { "language-culture-not-exist", new LocalizableString("language-culture-not-exist-message", requestCulture) }
                });
        }

        [Fact]
        public async Task Validate_request_when_exists_requested_language_culture()
        {
            // Arrange

            var languageProvider = Substitute.For<IAmLanguageProvider>();
            languageProvider
                .GetLanguageByCulture(Arg.Any<string>())
                .ReturnsForAnyArgs(Task.FromResult(new LanguageModel
                {
                    Culture = "en-US",
                    Id = Guid.NewGuid(),
                    Name = "en us"
                }));

            var sut = new AddOrUpdateContentValidator(languageProvider);

            // Act
            var result = await sut.IsValidAsync(new AddOrUpdateContent("culture", "key", "value"), CancellationToken.None);

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }
    }
}