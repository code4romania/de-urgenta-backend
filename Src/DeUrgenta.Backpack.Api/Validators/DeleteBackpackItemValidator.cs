using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

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
            //TODO: check if item exists
            throw new NotImplementedException();
        }
    }
}
