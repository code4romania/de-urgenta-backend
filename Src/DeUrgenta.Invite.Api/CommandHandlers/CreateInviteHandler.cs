using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using MediatR;
using InviteType = DeUrgenta.Invite.Api.Models.InviteType;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public class CreateInviteHandler : IRequestHandler<CreateInvite, Result<InviteModel>>
    {
        private readonly IValidateRequest<CreateInvite> _validator;
        private readonly DeUrgentaContext _context;

        public CreateInviteHandler(DeUrgentaContext context, IValidateRequest<CreateInvite> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<InviteModel>> Handle(CreateInvite request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<InviteModel>("Validation failed");
            }

            var invite = new Domain.Entities.Invite
            {
                InviteStatus = InviteStatus.Sent,
                SentOn = DateTime.Today,
                Type = (Domain.Entities.InviteType)request.Type,
                DestinationId = request.DestinationId
            };
            await _context.Invites.AddAsync(invite, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new InviteModel
            {
                Id = invite.Id,
                DestinationId = invite.DestinationId,
                Type = (InviteType)invite.Type,
                SentOn = invite.SentOn
            };
        }
    }
}
