using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

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
            //TODO: check if item exists
            throw new NotImplementedException();
        }
    }
}
