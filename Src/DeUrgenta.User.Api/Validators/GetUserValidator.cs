using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Queries;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.Validators
{
    public class GetUserValidator : IValidateRequest<GetUser>
    {
        private readonly DeUrgentaContext _context;

        public GetUserValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetUser request)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Sub == request.UserSub);

            return userExists ? ValidationResult.Ok : ValidationResult.GenericValidationError;
        }
    }
}