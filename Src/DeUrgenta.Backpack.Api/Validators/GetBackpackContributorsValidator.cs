using System;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class GetBackpackContributorsValidator : IValidateRequest<GetBackpackContributors>
    {
        private readonly DeUrgentaContext _context;

        public GetBackpackContributorsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public Task<bool> IsValidAsync(GetBackpackContributors request)
        {
            throw new NotImplementedException();
        }
    }
}