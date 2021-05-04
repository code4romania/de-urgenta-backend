using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class CreateBackpackValidator : IValidateRequest<CreateBackpack>
    {
        private readonly DeUrgentaContext _context;

        public CreateBackpackValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public Task<bool> IsValidAsync(CreateBackpack request)
        {
            throw new System.NotImplementedException();
        }
    }
}