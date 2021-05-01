using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class DeleteSafeLocation : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid SafeLocationId { get; }

        public DeleteSafeLocation(string userSub, Guid safeLocationId)
        {
            UserSub = userSub;
            SafeLocationId = safeLocationId;
        }
    }
}