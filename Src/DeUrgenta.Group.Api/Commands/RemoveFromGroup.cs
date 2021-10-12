using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class RemoveFromGroup : IRequest<Result<Unit, ValidationResult>>
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