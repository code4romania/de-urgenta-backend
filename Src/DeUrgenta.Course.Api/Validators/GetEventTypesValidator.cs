using System.Threading.Tasks;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventTypesValidator : IValidateRequest<GetEventTypes>
    {
        private readonly DeUrgentaContext _context;

        public GetEventTypesValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetEventTypes request)
        {
            return true;
        }
    }
}
