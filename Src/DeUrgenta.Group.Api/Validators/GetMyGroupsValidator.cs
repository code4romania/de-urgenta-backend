using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Queries;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.Validators
{
    public class GetMyGroupsValidator : IValidateRequest<GetMyGroups>
    {
        private readonly DeUrgentaContext _context;

        public GetMyGroupsValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(GetMyGroups request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);

            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}