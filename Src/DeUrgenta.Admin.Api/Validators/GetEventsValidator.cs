using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Admin.Api.Validators
{
    public class GetEventsValidator : IValidateRequest<GetEvents>
    {
        public Task<ValidationResult> IsValidAsync(GetEvents request, CancellationToken ct)
        {
            return Task.FromResult(ValidationResult.Ok);
        }
    }
}
