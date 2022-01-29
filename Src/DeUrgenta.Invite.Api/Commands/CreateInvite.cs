using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Invite.Api.Models;
using MediatR;

namespace DeUrgenta.Invite.Api.Commands
{
    public class CreateInvite : IRequest<Result<InviteModel, ValidationResult>>
    {
        public string UserSub { get; set; }
        public Guid DestinationId { get; set; }
        public InviteType Type { get; set; }

        public Guid UserId { get; set; }

        public CreateInvite(string sub, InviteRequest request)
        {
            UserSub = sub;
            DestinationId = request.DestinationId;
            Type = request.Type;
        }
    }
}