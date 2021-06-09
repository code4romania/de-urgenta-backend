using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.CommandHandlers
{
    public class RejectGroupInviteHandler : IRequestHandler<RejectGroupInvite,Result>
    {
        private readonly IValidateRequest<RejectGroupInvite> _validator;
        private readonly DeUrgentaContext _context;

        public RejectGroupInviteHandler(IValidateRequest<RejectGroupInvite> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(RejectGroupInvite request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var invite = await _context
                .GroupInvites
                .FirstAsync(gi => 
                    gi.InvitationReceiver.Sub == request.UserSub 
                    && gi.Id == request.GroupInviteId, cancellationToken);

            _context.GroupInvites.Remove(invite);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}