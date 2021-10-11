using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public class AcceptInviteHandler : IRequestHandler<AcceptInvite, Result<AcceptInviteModel>>
    {
        private readonly IValidateRequest<AcceptInvite> _validator;
        private readonly DeUrgentaContext _context;
        private readonly InviteHandlerFactory _handlerFactory;

        public AcceptInviteHandler(DeUrgentaContext context, IValidateRequest<AcceptInvite> validator, InviteHandlerFactory handlerFactory)
        {
            _validator = validator;
            _handlerFactory = handlerFactory;
            _context = context;
        }

        public async Task<Result<AcceptInviteModel>> Handle(AcceptInvite request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<AcceptInviteModel>("Validation failed");
            }

            var invite = await _context.Invites.FirstAsync(i => i.Id == request.InviteId, cancellationToken);
            var handlerInstance = _handlerFactory.GetHandlerInstance(invite.Type);

            return await handlerInstance.HandleAsync(request, cancellationToken);
        }
    }
}
