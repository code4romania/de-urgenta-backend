using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using MediatR;
using InviteType = DeUrgenta.Invite.Api.Models.InviteType;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public class CreateInviteHandler : IRequestHandler<CreateInvite, Result<InviteModel, ValidationResult>>
    {
        private readonly IValidateRequest<CreateInvite> _validator;
        private readonly DeUrgentaContext _context;

        public CreateInviteHandler(DeUrgentaContext context, IValidateRequest<CreateInvite> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<InviteModel, ValidationResult>> Handle(CreateInvite request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var invite = new Domain.Api.Entities.Invite
            {
                InviteStatus = InviteStatus.Sent,
                SentOn = DateTime.Today,
                Type = (Domain.Api.Entities.InviteType)request.Type,
                DestinationId = request.DestinationId
            };
            await _context.Invites.AddAsync(invite, ct);
            await _context.SaveChangesAsync(ct);

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
