using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class AcceptGroupInvite : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid GroupInviteId { get; }

        public AcceptGroupInvite(string userSub, Guid groupInviteId)
        {
            UserSub = userSub;
            GroupInviteId = groupInviteId;
        }
    }
}