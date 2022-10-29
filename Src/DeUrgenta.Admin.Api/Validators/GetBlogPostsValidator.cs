using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Admin.Api.Validators
{
    public class GetBlogPostsValidator : IValidateRequest<GetBlogPosts>
    {
        public Task<ValidationResult> IsValidAsync(GetBlogPosts request, CancellationToken ct)
        {
            return Task.FromResult(ValidationResult.Ok);
        }
    }
}