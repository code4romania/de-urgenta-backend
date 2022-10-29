using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class DeleteGroupValidator : IValidateRequest<DeleteGroup>
    {
        private readonly DeUrgentaContext _context;

        public DeleteGroupValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(DeleteGroup request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);

            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isPartOfTheGroup = await _context.UsersToGroups.AnyAsync(utg => utg.UserId == user.Id 
                && utg.GroupId == request.GroupId, ct);
            if (!isPartOfTheGroup)
            {
                return ValidationResult.GenericValidationError;
            }

            var isGroupAdmin = await _context.Groups.AnyAsync(g => g.Admin.Id == user.Id && g.Id == request.GroupId, ct);

            if (!isGroupAdmin)
            {
                return new LocalizableValidationError("cannot-delete-group", "only-group-admin-can-delete-group-message");
            }

            return ValidationResult.Ok;
        }
    }
}