using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Queries;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class GetAdministeredGroupsValidator : IValidateRequest<GetAdministeredGroups>
    {
        private readonly DeUrgentaContext _context;

        public GetAdministeredGroupsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetAdministeredGroups request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);

            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}