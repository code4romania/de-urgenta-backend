using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Invite.Api.Models;
using MediatR;

namespace DeUrgenta.Invite.Api.Commands
{
    public class AcceptGroupInvite : IRequest<Result<AcceptInviteModel>>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }

        public AcceptGroupInvite(string userSub, Guid groupId)
        {
            UserSub = userSub;
            GroupId = groupId;
        }
    }
}