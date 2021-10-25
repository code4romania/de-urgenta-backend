using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptGroupInviteValidator : IValidateRequest<AcceptGroupInvite>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;
        private readonly GroupsConfig _config;

        public AcceptGroupInviteValidator(DeUrgentaContext context, IamI18nProvider i18nProvider, IOptions<GroupsConfig> config)
        {
            _context = context;
            _i18nProvider = i18nProvider;
            _config = config.Value;
        }

        public async Task<ValidationResult> IsValidAsync(AcceptGroupInvite request)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId);
            if (group == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub);

            var noOfGroupsUserIsAMemberOf = await _context.UsersToGroups
                .CountAsync(u => u.UserId == user.Id);

            if (noOfGroupsUserIsAMemberOf >= _config.MaxJoinedGroupsPerUser)
            {
                return new DetailedValidationError("Cannot accept invite", "Current maximum number of groups user is part of is reached.");
            }

            var noOfGroupMembers = await _context.UsersToGroups
                .CountAsync(u => u.GroupId == request.GroupId);
            
            if (noOfGroupMembers >= _config.MaxUsers)
            {
                return new DetailedValidationError("Cannot accept invite", "Current maximum number of group members is reached.");
            }

            var userIsAlreadyAMember = await _context.UsersToGroups
                .AnyAsync(u => u.UserId == user.Id && u.GroupId == request.GroupId);

            if (userIsAlreadyAMember)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}