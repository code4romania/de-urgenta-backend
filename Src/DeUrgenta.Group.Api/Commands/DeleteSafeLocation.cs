using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class DeleteSafeLocation : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid LocationId { get; }

        public DeleteSafeLocation(string userSub, Guid locationId)
        {
            UserSub = userSub;
            LocationId = locationId;
        }
    }
}