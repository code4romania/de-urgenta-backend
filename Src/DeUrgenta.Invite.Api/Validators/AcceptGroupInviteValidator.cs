using System.Threading.Tasks;
using DeUrgenta.Domain;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptGroupInviteValidator : IAcceptInviteValidator
    {
        private DeUrgentaContext _context;
        private readonly GroupsConfig _config;

        public AcceptGroupInviteValidator(DeUrgentaContext context, IOptions<GroupsConfig> config)
        {
            _context = context;
            _config = config.Value;
        }

        public async Task<bool> ValidateAsync(AcceptInvite request)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == request.DestinationId);
            if (group == null)
            {
                return false;
            }

            var noOfGroupsUserIsAMemberOf = await _context.UsersToGroups
                .CountAsync(u => u.UserId == request.UserId);

            if (noOfGroupsUserIsAMemberOf >= _config.MaxJoinedGroupsPerUser)
            {
                return false;
            }

            var noOfGroupMembers = await _context.UsersToGroups
                .CountAsync(u => u.GroupId == request.DestinationId);
            if (noOfGroupMembers >= _config.UsersLimit)
            {
                return false;
            }

            var userIsAlreadyAMember = await _context.UsersToGroups
                .AnyAsync(u => u.UserId == request.UserId
                                     && u.GroupId == request.DestinationId);
            if (userIsAlreadyAMember)
            {
                return false;
            }

            return true;
        }
    }
}