using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class GetBackpacksValidator : IValidateRequest<GetBackpacks>
    {
        private readonly DeUrgentaContext _context;

        public GetBackpacksValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public Task<bool> IsValidAsync(GetBackpacks request)
        {
            throw new System.NotImplementedException();
        }
    }
}