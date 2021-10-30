using System;
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
        private readonly IamI18nProvider _i18nProvider;

        public AddOrUpdateContentValidatorShould()
        {
            _i18nProvider = Substitute.For<IamI18nProvider>();
            _i18nProvider.Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");
        }

        [Fact]
        public async Task Invalidate_request_when_not_existing_language_culture()
        {
            // Arrange

            var languageProvider = Substitute.For<IAmLanguageProvider>();
            languageProvider
                .GetLanguageByCulture(Arg.Any<string>())
                .ReturnsForAnyArgs(Task.FromResult<LanguageModel>(null));

            var sut = new AddOrUpdateContentValidator(languageProvider, _i18nProvider);

            // Act
            var result = await sut.IsValidAsync(new AddOrUpdateContent("culture", "key", "value"));

            // Assert
            result.Should().BeOfType<DetailedValidationError>();
            await _i18nProvider.Received(1).Localize(Arg.Is("language-culture-not-exist"));
            await _i18nProvider.Received(1).Localize(Arg.Is("language-culture-not-exist-message"), Arg.Is("culture"));
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

            var sut = new AddOrUpdateContentValidator(languageProvider, _i18nProvider);

            // Act
            var result = await sut.IsValidAsync(new AddOrUpdateContent("culture", "key", "value"));

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }

    }
}