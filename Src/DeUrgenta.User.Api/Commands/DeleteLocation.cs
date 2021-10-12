using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class DeleteLocation : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid LocationId { get; }

        public DeleteLocation(string userSub, Guid locationId)
        {
            UserSub = userSub;
            LocationId = locationId;
        }
    }
}