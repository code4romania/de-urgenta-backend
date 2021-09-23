using System.Threading.Tasks;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventTypesValidator : IValidateRequest<GetEventTypes>
    {
        public Task<bool> IsValidAsync(GetEventTypes request)
        {
            return Task.FromResult(true);
        }
    }
}
