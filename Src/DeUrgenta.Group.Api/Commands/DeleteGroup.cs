using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class DeleteGroup : IRequest<Result<Unit, ValidationResult>>
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