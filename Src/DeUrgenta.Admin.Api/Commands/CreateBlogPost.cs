using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Models;
using MediatR;

namespace DeUrgenta.Admin.Api.Commands
{
    public class CreateBlogPost : IRequest<Result<BlogPostModel>>
    {
        public BlogPostRequest Blog { get; }

        public CreateBlogPost(BlogPostRequest blog)
        {
            Blog = blog;
        }
    }
}