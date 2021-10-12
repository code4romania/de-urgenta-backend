using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.User.Api.Validators
{
    public class AcceptGroupInviteValidator : IValidateRequest<AcceptGroupInvite>
    {
        private readonly DeUrgentaContext _context;
        private readonly IOptions<GroupsConfig> _options;

        public AcceptGroupInviteValidator(DeUrgentaContext context, IOptions<GroupsConfig> options)
        {
            _context = context;
            _options = options;
        }

        public async Task<ValidationResult> IsValidAsync(AcceptGroupInvite request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);

            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var invite = await _context
                .GroupInvites
                .FirstOrDefaultAsync(bi => bi.InvitationReceiver.Sub == request.UserSub && bi.Id == request.GroupInviteId);

            if (invite is null)
            {
                return ValidationResult.GenericValidationError;
            }

            var config = _options.Value;
            
            var exceedsLimit = await _context.UsersToGroups
                .Where(x => x.GroupId == invite.GroupId)
                .GroupBy(x => x.GroupId)
                .Where(g => g.Count() >= config.UsersLimit)
                .AnyAsync();
            
            if (exceedsLimit)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}