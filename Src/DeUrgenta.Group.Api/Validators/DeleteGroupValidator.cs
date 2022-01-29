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

        public async Task<ValidationResult> IsValidAsync(DeleteGroup request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);

            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isPartOfTheGroup = await _context.UsersToGroups.AnyAsync(utg => utg.UserId == user.Id && utg.GroupId == request.GroupId);
            if (!isPartOfTheGroup)
            {
                return ValidationResult.GenericValidationError;
            }

            var isGroupAdmin = await _context.Groups.AnyAsync(g => g.Admin.Id == user.Id && g.Id == request.GroupId);

            if (!isGroupAdmin)
            {
                return new LocalizableValidationError("cannot-delete-group", "only-group-admin-can-delete-group-message");
            }

            return ValidationResult.Ok;
        }
    }
}