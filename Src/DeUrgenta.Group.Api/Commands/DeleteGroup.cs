using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class DeleteGroup : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }

        public DeleteGroup(string userSub, Guid groupId)
        {
            UserSub = userSub;
            GroupId = groupId;
        }
    }
}