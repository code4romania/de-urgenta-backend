using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Admin.Api.Validators
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
