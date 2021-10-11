using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Invite.Api.Models;
using MediatR;

namespace DeUrgenta.Invite.Api.Commands
{
    public class AcceptInvite : IRequest<Result<AcceptInviteModel>>
    {
        public string UserSub { get; set; }
        public Guid InviteId { get; set; }

        public Guid UserId { get; set; }
        public Guid DestinationId { get; set; }

        public AcceptInvite(string sub, Guid inviteId)
        {
            UserSub = sub;
            InviteId = inviteId;
        }
    }
}