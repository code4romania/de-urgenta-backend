using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class RemoveFromGroupValidator : IValidateRequest<RemoveFromGroup>
    {
        private readonly DeUrgentaContext _context;

        public RemoveFromGroupValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(RemoveFromGroup request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            
            if (user == null || targetUser == null)
            {
                return ValidationResult.GenericValidationError;
            }

            if (user.Id == request.UserId)
            {
                return ValidationResult.GenericValidationError;
            }

            var isAdmin =await _context.Groups.AnyAsync(g => g.Admin.Id == user.Id);
            
            if (!isAdmin)
            {
                return ValidationResult.GenericValidationError;
            }

            bool requestedUserIsInGroup = await _context
                .UsersToGroups
                .AnyAsync(utg => utg.Group.Id == request.GroupId && utg.User.Id == request.UserId);

            if (!requestedUserIsInGroup)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}