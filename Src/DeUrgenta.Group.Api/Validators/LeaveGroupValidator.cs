using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class LeaveGroupValidator : IValidateRequest<LeaveGroup>
    {
        private readonly DeUrgentaContext _context;

        public LeaveGroupValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(LeaveGroup request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);

            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isAdmin = await _context.Groups.AnyAsync(g => g.Admin.Id == user.Id && g.Id == request.GroupId, ct);
            if (isAdmin)
            {
                return new LocalizableValidationError("cannot-leave-group","cannot-leave-administered-group-message");
            }

            var isPartOfGroup = await _context.UsersToGroups
                .AnyAsync(utg => utg.User.Id == user.Id && utg.Group.Id == request.GroupId, ct);

            if (!isPartOfGroup)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}