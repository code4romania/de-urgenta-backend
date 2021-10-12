using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class InviteToGroup : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }
        public Guid UserId { get; }

        public InviteToGroup(string userSub, Guid groupId, Guid userId)
        {
            UserSub = userSub;
            GroupId = groupId;
            UserId = userId;
        }
    }
}