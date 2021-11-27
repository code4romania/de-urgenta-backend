using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Validators;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.I18n.Service.Models;
using DeUrgenta.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace DeUrgenta.Admin.Api.Tests.Validators
{
    [Collection(TestsConstants.DbCollectionName)]
    public class UpdateBlogPostValidatorShould
    {
        private readonly DeUrgentaContext _dbContext;

        public UpdateBlogPostValidatorShould(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
        }

        [Fact]
        public async Task Invalidate_request_when_blog_post_does_not_exists()
        {
            // Arrange
            var sut = new UpdateBlogPostValidator(_dbContext);

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
            var result = await sut.IsValidAsync(new UpdateBlogPost(blogPostId, new BlogPostRequest
            {
                Author = "Test",
                Title = "Test",
                ContentBody = "Test"
            }));

            // Assert
            result
                .Should()
                .BeOfType<LocalizableValidationError>()
                .Which.Messages
                .Should()
                .BeEquivalentTo(new Dictionary<LocalizableString, LocalizableString>
                {
                    { "blogpost-not-exist",new LocalizableString("blogpost-not-exist-message", blogPostId)}
                });
        }

        [Fact]
        public async Task Validate_request_when_blog_post_exists()
        {
            // Arrange
            var sut = new UpdateBlogPostValidator(_dbContext);

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
            var result = await sut.IsValidAsync(new UpdateBlogPost(blogPost.Id, new BlogPostRequest
            {
                Author = "Test",
                Title = "Test",
                ContentBody = "Test"
            }));

            // Assert
            result.Should().BeOfType<ValidationPassed>();
        }

    }
}