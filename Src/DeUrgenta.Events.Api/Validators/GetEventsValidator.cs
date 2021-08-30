using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Events.Api.Queries;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventsValidator : IValidateRequest<GetEvents>
    {
        private readonly DeUrgentaContext _context;

        public GetEventsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetEvents request)
        {
            return true;
        }
    }
}
