using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Queries
{
    public class GetGroupSafeLocations : IRequest<Result<IImmutableList<SafeLocationResponseModel>, ValidationResult>>
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