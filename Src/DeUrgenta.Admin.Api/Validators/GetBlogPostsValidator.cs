using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Admin.Api.Validators
{
    public class GetBlogPostsValidator : IValidateRequest<GetBlogPosts>
    {
        public Task<bool> IsValidAsync(GetBlogPosts request)
        {
            return Task.FromResult(true);
        }
    }
}