using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class InviteToBackpackContributorsValidator : IValidateRequest<InviteToBackpackContributors>
    {
        private readonly DeUrgentaContext _context;

        public InviteToBackpackContributorsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public Task<bool> IsValidAsync(InviteToBackpackContributors request)
        {
            throw new NotImplementedException();
        }
    }
}