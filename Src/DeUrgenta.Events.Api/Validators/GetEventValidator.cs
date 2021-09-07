using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Events.Api.Queries;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Events.Api.Validators
{
    public class GetEventValidator : IValidateRequest<GetEvent>
    {
        private readonly DeUrgentaContext _context;

        public GetEventValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetEvent request)
        {
            if (request.ModelRequest != null)
            {
                var isValid = await _context.EventTypes.AnyAsync(x => x.Id == request.ModelRequest.EventTypeId);
                if (!isValid) return false;
            }

            return true;
        }
    }
}
