using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
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

        public async Task<bool> IsValidAsync(CreateInvite request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }

            request.UserId = user.Id;

            var inviteTypeValidator = _validatorFactory.GetInstance(request.Type);

            return await inviteTypeValidator.ValidateAsync(request);
        }
    }
}
