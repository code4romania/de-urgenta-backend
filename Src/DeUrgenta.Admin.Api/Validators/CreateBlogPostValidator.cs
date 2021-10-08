using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Admin.Api.Validators
{
    public class CreateBlogPostValidator : IValidateRequest<CreateBlogPost>
    {
        public Task<bool> IsValidAsync(CreateBlogPost request)
        {
            return Task.FromResult(true);
        }
    }
}