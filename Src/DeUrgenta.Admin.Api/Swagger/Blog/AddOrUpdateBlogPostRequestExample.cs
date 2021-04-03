using DeUrgenta.Admin.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Swagger.Blog
{
    public class AddOrUpdateBlogPostRequestExample : IExamplesProvider<BlogPostRequest>
    {
        public BlogPostRequest GetExamples()
        {
            return new()
            {
                Title = "My first blog post",
                Author = "Alber Einstein",
                ContentBody = "<h1>Lorem ipsum</h1>"
            };
        }
    }
}
