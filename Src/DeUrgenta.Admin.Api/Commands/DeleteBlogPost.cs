using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class DeleteBlogPost : IRequest<Result>
    {
        public Guid BlogPostId { get; }

        public DeleteBlogPost(Guid blogPostId)
        {
            BlogPostId = blogPostId;
        }
    }
}