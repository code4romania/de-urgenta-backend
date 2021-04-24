using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Queries
{
    public class GetGroupSafeLocations : IRequest<Result<IImmutableList<SafeLocationModel>>>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }

        public GetGroupSafeLocations(string userSub, Guid groupId)
        {
            UserSub = userSub;
            GroupId = groupId;
        }
    }
}