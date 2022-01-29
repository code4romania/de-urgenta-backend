using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class DeleteBlogPost : IRequest<Result<Unit, ValidationResult>>
    {
        public Guid BlogPostId { get; }

        public DeleteBlogPost(Guid blogPostId)
        {
            BlogPostId = blogPostId;
        }
    }
}