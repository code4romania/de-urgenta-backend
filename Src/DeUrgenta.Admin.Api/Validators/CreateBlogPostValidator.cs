using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Admin.Api.Validators
{
    public class CreateBlogPostValidator : IValidateRequest<CreateBlogPost>
    {
        public Task<ValidationResult> IsValidAsync(CreateBlogPost request, CancellationToken ct)
        {
            return Task.FromResult(ValidationResult.Ok);
        }
    }
}