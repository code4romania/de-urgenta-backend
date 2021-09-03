using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class UpdateEventValidator : IValidateRequest<UpdateEvent>
    {
        private readonly DeUrgentaContext _context;

        public UpdateEventValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(UpdateEvent request)
        {
            var ev = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.EventId);
            if (ev == null)
            {
                return false;
            }

            return true;
        }
    }
}
