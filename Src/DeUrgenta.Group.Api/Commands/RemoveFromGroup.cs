using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class RemoveFromGroup : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }
        public Guid UserId { get; }

        public RemoveFromGroup(string userSub, Guid groupId, Guid userId)
        {
            UserSub = userSub;
            GroupId = groupId;
            UserId = userId;
        }
    }
}