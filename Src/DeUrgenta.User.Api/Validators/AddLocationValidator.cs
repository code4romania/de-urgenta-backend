using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.User.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.Validators
{
    public class AddLocationValidator : IValidateRequest<AddLocation>
    {
        private readonly DeUrgentaContext _context;

        public AddLocationValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(AddLocation request)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Sub == request.UserSub);

            return userExists ? ValidationResult.Ok : ValidationResult.GenericValidationError;
        }
    }
}