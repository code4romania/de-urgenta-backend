using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class LeaveGroup : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }

        public LeaveGroup(string userSub, Guid groupId)
        {
            UserSub = userSub;
            GroupId = groupId;
        }
    }
}