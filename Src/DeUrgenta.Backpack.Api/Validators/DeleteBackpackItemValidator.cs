using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class DeleteBackpackItemValidator : IValidateRequest<DeleteBackpackItem>
    {
        private readonly DeUrgentaContext _context;
        public DeleteBackpackItemValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<bool> IsValidAsync(DeleteBackpackItem request)
        {
            return await _context.BackpackItems.AnyAsync(x => x.Id == request.ItemId);
        }
    }
}
