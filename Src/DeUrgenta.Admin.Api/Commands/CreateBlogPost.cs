using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class CreateBlogPost : IRequest<Result<BlogPostModel, ValidationResult>>
    {
        public BlogPostRequest Blog { get; }

        public CreateBlogPost(BlogPostRequest blog)
        {
            Blog = blog;
        }
    }
}