using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class AddBackpackItemValidator : IValidateRequest<AddBackpackItem>
    {
        private readonly DeUrgentaContext _context;
        public AddBackpackItemValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<bool> IsValidAsync(AddBackpackItem request)
        {
            return await _context.Backpacks.AnyAsync(x => x.Id == request.BackpackId);
        }
    }
}
