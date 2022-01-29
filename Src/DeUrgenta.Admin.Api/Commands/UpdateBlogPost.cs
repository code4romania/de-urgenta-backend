using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class UpdateBlogPost : IRequest<Result<BlogPostModel, ValidationResult>>
    {
        public Guid BlogPostId { get; }
        public BlogPostRequest BlogPost { get; }

        public UpdateBlogPost(Guid blogPostId, BlogPostRequest blogPost)
        {
            BlogPostId = blogPostId;
            BlogPost = blogPost;
        }
    }
}