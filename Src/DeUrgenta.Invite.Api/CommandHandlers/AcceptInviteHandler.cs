using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using InviteType = DeUrgenta.Domain.Entities.InviteType;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public class AcceptInviteHandler : IRequestHandler<AcceptInvite, Result<AcceptInviteModel, ValidationResult>>
    {
        private readonly IValidateRequest<AcceptInvite> _validator;
        private readonly DeUrgentaContext _context;
        private readonly IMediator _mediator;

        public AcceptInviteHandler(DeUrgentaContext context, IValidateRequest<AcceptInvite> validator, IMediator mediator)
        {
            _validator = validator;
            _mediator = mediator;
            _context = context;
        }

        public async Task<Result<AcceptInviteModel, ValidationResult>> Handle(AcceptInvite request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var invite = await _context.Invites.FirstAsync(i => i.Id == request.InviteId, cancellationToken);

            if (invite.Type == InviteType.Group)
            {
                var acceptGroupInvite = new AcceptGroupInvite(request.UserSub, invite.DestinationId);
                return await _mediator.Send(acceptGroupInvite, cancellationToken);
            }
            else
            {
                var acceptBackpackInvite = new AcceptBackpackInvite(request.UserSub, invite.DestinationId);
                return await _mediator.Send(acceptBackpackInvite, cancellationToken);
            }
        }
    }
}
