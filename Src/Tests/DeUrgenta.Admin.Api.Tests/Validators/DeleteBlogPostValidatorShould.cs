using System;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class DeleteBlogPostValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;
        private readonly IamI18nProvider _i18nProvider;

        public DeleteBlogPostValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;

            _i18nProvider = Substitute.For<IamI18nProvider>();
            _i18nProvider
                .Localize(Arg.Any<string>(), Arg.Any<object[]>())
                .ReturnsForAnyArgs("some message");
        }

        [Fact]
        public async Task Invalidate_request_when_blog_post_does_not_exists()
        {
            // Arrange
            var sut = new DeleteBlogPostValidator(_dbContext, _i18nProvider);

            await _dbContext.AddAsync(new BlogPost
            {
                Author = "Test",
                Title = "Test",
                ContentBody = "Test",
                PublishedOn = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();

            // Act
            var blogPostId = Guid.NewGuid();
            var result = await sut.IsValidAsync(new DeleteBlogPost(blogPostId));

            // Assert
            result.Should().BeOfType<DetailedValidationError>();

            await _i18nProvider.Received(1).Localize(Arg.Is("blogpost-not-exist"));
            await _i18nProvider.Received(1).Localize(Arg.Is("blogpost-not-exist-message"), Arg.Is(blogPostId));
        }

        [Fact]
        public async Task Validate_request_when_blog_post_exists()
        {
            // Arrange
            var sut = new DeleteBlogPostValidator(_dbContext, _i18nProvider);

            var blogPost = new BlogPost
            {
                Author = "Test",
                Title = "Test",
                ContentBody = "Test",
                PublishedOn = DateTime.UtcNow
            };
            await _dbContext.AddAsync(blogPost);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await sut.IsValidAsync(new DeleteBlogPost(blogPost.Id));

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }

    }
}