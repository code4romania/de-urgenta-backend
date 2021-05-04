using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class DeleteBackpackValidator : IValidateRequest<DeleteBackpack>
    {
        private readonly DeUrgentaContext _context;

        public DeleteBackpackValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public Task<bool> IsValidAsync(DeleteBackpack request)
        {
            throw new NotImplementedException();
        }
    }
}