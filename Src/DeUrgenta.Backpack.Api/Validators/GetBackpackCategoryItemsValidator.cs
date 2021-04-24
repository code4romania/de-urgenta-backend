using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class GetBackpackCategoryItemsValidator : IValidateRequest<GetBackpackCategoryItems>
    {
        private readonly DeUrgentaContext _context;

        public GetBackpackCategoryItemsValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<bool> IsValidAsync(GetBackpackCategoryItems request)
        {
            return await _context.Backpacks.AnyAsync(x => x.Id == request.BackpackId);
            //TODO: check if category is valid
        }
    }
}
