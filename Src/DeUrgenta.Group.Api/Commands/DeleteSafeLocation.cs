using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class DeleteSafeLocation : IRequest<Result<Unit, ValidationResult>>
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