using System;
using DeUrgenta.Admin.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Swagger.Blog
{
    public class AddOrUpdateBlogPostResponseExample : IExamplesProvider<BlogPostModel>
    {
        public BlogPostModel GetExamples()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Title = "My first blog post",
                Author = "Alber Einstein",
                ContentBody = "<h1>Lorem ipsum</h1>",
                PublishedOn = DateTime.Today.AddDays(-42)
            };
        }
    }
}