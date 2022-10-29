using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class UpdateGroupValidator : IValidateRequest<UpdateGroup>
    {
        private readonly DeUrgentaContext _context;

        public UpdateGroupValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(UpdateGroup request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);

            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var groupExists = await _context.Groups.AnyAsync(g => g.Id == request.GroupId, ct);
            if (!groupExists)
            {
                return ValidationResult.GenericValidationError;
            }

            var isGroupAdmin = await _context
                .Groups
                .AnyAsync(g => g.Id == request.GroupId && g.Admin.Id == user.Id, ct);

            if (!isGroupAdmin)
            {
                return new LocalizableValidationError("cannot-update-group","only-admin-cab-update-group");
            }

            return ValidationResult.Ok;
        }
    }
}