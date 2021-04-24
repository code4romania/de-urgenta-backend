using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class GetBackpackItemsValidator : IValidateRequest<GetBackpackItems>
    {
        private readonly DeUrgentaContext _context;

        public GetBackpackItemsValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<bool> IsValidAsync(GetBackpackItems request)
        {
            return await _context.Backpacks.AnyAsync(x => x.Id == request.BackpackId);
        }
    }
}
