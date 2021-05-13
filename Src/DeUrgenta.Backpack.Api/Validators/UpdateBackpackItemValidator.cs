using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class UpdateBackpackItemValidator : IValidateRequest<UpdateBackpackItem>
    {
        private readonly DeUrgentaContext _context;
        public UpdateBackpackItemValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<bool> IsValidAsync(UpdateBackpackItem request)
        {
            return await _context.BackpackItem.AnyAsync(x => x.Id == request.ItemId);
        }
    }
}
