using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptGroupInviteValidator : IValidateRequest<AcceptGroupInvite>
    {
        private readonly DeUrgentaContext _context;
        private readonly GroupsConfig _config;

        public AcceptGroupInviteValidator(DeUrgentaContext context, IOptions<GroupsConfig> config)
        {
            _context = context;
            _config = config.Value;
        }

        public async Task<ValidationResult> IsValidAsync(AcceptGroupInvite request, CancellationToken ct)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId, ct);
            if (group == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, ct);

            var noOfGroupsUserIsAMemberOf = await _context.UsersToGroups
                .CountAsync(u => u.UserId == user.Id, ct);

            if (noOfGroupsUserIsAMemberOf >= _config.MaxJoinedGroupsPerUser)
            {
                return new LocalizableValidationError("cannot-accept-invite", "max-group-per-user-reached");
            }

            var noOfGroupMembers = await _context.UsersToGroups
                .CountAsync(u => u.GroupId == request.GroupId, ct);

            if (noOfGroupMembers >= _config.MaxUsers)
            {
                return new LocalizableValidationError("cannot-accept-invite", "max-group-members-reached");
            }

            var userIsAlreadyAMember = await _context.UsersToGroups
                .AnyAsync(u => u.UserId == user.Id && u.GroupId == request.GroupId, ct);

            if (userIsAlreadyAMember)
            {
                return new LocalizableValidationError("cannot-accept-invite", "already-a-group-member-message");
            }

            return ValidationResult.Ok;
        }
    }
}