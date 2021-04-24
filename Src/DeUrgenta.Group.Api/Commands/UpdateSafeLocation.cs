using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class UpdateSafeLocation : IRequest<Result<SafeLocationModel>>
    {
        public string UserSub { get; }
        public Guid GroupId { get; }
        public Guid SafeLocationId { get; }
        public SafeLocationRequest SafeLocation { get; }

        public UpdateSafeLocation(string userSub, Guid groupId, Guid safeLocationId, SafeLocationRequest safeLocation)
        {
            UserSub = userSub;
            GroupId = groupId;
            SafeLocationId = safeLocationId;
            SafeLocation = safeLocation;
        }
    }
}