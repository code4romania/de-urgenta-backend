using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Queries
{
    public class GetGroupMembers : IRequest<Result<IImmutableList<GroupMemberModel>, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }

        public GetGroupMembers(string userSub, Guid groupId)
        {
            UserSub = userSub;
            GroupId = groupId;
        }
    }
}