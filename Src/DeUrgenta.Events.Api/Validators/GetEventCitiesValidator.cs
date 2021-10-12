using System.Threading.Tasks;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventCitiesValidator : IValidateRequest<GetEventCities>
    {
        private readonly DeUrgentaContext _context;

        public GetEventCitiesValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetEventCities request)
        {
            if (!await _context.EventTypes.AnyAsync(x => x.Id == request.EventTypeId))
            {
                return ValidationResult.GenericValidationError;
            }


            return ValidationResult.Ok;
        }
    }
}
