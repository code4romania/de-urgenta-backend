using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class UpdateSafeLocation : IRequest<Result<SafeLocationResponseModel, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid SafeLocationId { get; }
        public SafeLocationRequest SafeLocation { get; }

        public UpdateSafeLocation(string userSub, Guid safeLocationId, SafeLocationRequest safeLocation)
        {
            UserSub = userSub;
            SafeLocationId = safeLocationId;
            SafeLocation = safeLocation;
        }
    }
}