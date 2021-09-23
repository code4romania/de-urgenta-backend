using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Admin.Api.Validators
{
    public class GetEventsValidator : IValidateRequest<GetEvents>
    {
        public Task<bool> IsValidAsync(GetEvents request)
        {
            return Task.FromResult(true);
        }
    }
}
