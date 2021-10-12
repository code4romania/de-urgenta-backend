using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Invite.Api.Models;
using MediatR;

namespace DeUrgenta.Invite.Api.Commands
{
    public class AcceptBackpackInvite : IRequest<Result<AcceptInviteModel>>
    {
        public string UserSub { get; set; }
        public Guid BackpackId { get; set; }

        public AcceptBackpackInvite(string userSub, Guid backpackId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
        }
    }
}