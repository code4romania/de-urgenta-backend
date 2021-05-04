using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class UpdateBackpackValidator : IValidateRequest<UpdateBackpack>
    {
        private readonly DeUrgentaContext _context;

        public UpdateBackpackValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public Task<bool> IsValidAsync(UpdateBackpack request)
        {
            throw new NotImplementedException();
        }
    }
}