using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class RejectGroupInvite : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid GroupInviteId { get; }

        public RejectGroupInvite(string userSub, Guid groupInviteId)
        {
            UserSub = userSub;
            GroupInviteId = groupInviteId;
        }
    }
}