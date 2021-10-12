using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Invite.Api.Models;
using MediatR;

namespace DeUrgenta.Invite.Api.Commands
{
    public class AcceptBackpackInvite : IRequest<Result<AcceptInviteModel>>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }

        public AcceptBackpackInvite(string userSub, Guid backpackId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
        }
    }
}