using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventTypesValidator : IValidateRequest<GetEventTypes>
    {
        public Task<ValidationResult> IsValidAsync(GetEventTypes request, CancellationToken ct)
        {
            return Task.FromResult(ValidationResult.Ok);
        }
    }
}
