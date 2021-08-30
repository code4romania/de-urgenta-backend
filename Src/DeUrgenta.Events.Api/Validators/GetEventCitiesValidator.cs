using System.Threading.Tasks;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventCitiesValidator : IValidateRequest<GetEventCities>
    {
        private readonly DeUrgentaContext _context;

        public GetEventCitiesValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetEventCities request)
        {
            return true;
        }
    }
}
