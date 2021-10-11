using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Invite.Api.Commands;
using Microsoft.EntityFrameworkCore;
using InviteType = DeUrgenta.Invite.Api.Models.InviteType;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptInviteValidator : IValidateRequest<AcceptInvite>
    {
        private readonly DeUrgentaContext _context;
        private readonly InviteValidatorFactory _validatorFactory;

        public AcceptInviteValidator(DeUrgentaContext context, InviteValidatorFactory validatorFactory)
        {
            _context = context;
            _validatorFactory = validatorFactory;
        }

        public async Task<bool> IsValidAsync(AcceptInvite request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }

            var invite = await _context.Invites.FirstOrDefaultAsync(i => i.Id == request.InviteId);
            if (invite == null)
            {
                return false;
            }
            if (invite.InviteStatus == InviteStatus.Accepted)
            {
                return false;
            }

            request.UserId = user.Id;
            request.DestinationId = invite.DestinationId;

            var inviteTypeValidator = _validatorFactory.GetAcceptValidatorInstance((InviteType)invite.Type);
            return await inviteTypeValidator.ValidateAsync(request);
        }
    }
}
