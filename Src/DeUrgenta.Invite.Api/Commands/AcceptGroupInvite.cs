using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Invite.Api.Models;
using MediatR;

namespace DeUrgenta.Invite.Api.Commands
{
    public class AcceptGroupInvite : IRequest<Result<AcceptInviteModel>>
    {
        public string UserSub { get; set; }
        public Guid GroupId { get; set; }

        public AcceptGroupInvite(string userSub, Guid groupId)
        {
            UserSub = userSub;
            GroupId = groupId;
        }
    }
}