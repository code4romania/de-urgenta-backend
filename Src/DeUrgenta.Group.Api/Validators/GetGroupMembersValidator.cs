using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Queries;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class GetGroupMembersValidator : IValidateRequest<GetGroupMembers>
    {
        private readonly DeUrgentaContext _context;

        public GetGroupMembersValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetGroupMembers request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);

            if (user == null)
            {
                return false;
            }

            var isPartOfGroup = await _context
                .UsersToGroups
                .AnyAsync(utg => utg.User.Id == user.Id && utg.Group.Id == request.GroupId);

            if (!isPartOfGroup)
            {
                return false;
            }

            return true;
        }
    }
}