using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class GetMyBackpacksValidator : IValidateRequest<GetMyBackpacks>
    {
        private readonly DeUrgentaContext _context;

        public GetMyBackpacksValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public Task<bool> IsValidAsync(GetMyBackpacks request)
        {
            throw new System.NotImplementedException();
        }
    }
}