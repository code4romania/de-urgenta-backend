using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Invite.Api.Commands;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Invite.Api.Validators
{
    public class CreateInviteValidator : IValidateRequest<CreateInvite>
    {
        private readonly DeUrgentaContext _context;
        private readonly InviteValidatorFactory _validatorFactory;

        public CreateInviteValidator(DeUrgentaContext context, InviteValidatorFactory validatorFactory)
        {
            _context = context;
            _validatorFactory = validatorFactory;
        }

        public async Task<ValidationResult> IsValidAsync(CreateInvite request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            request.UserId = user.Id;

            var inviteTypeValidator = _validatorFactory.GetCreateValidatorInstance(request.Type);

            return await inviteTypeValidator.ValidateAsync(request);
        }
    }
}
