using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class AcceptBackpackInvite : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid BackpackInviteId { get; }

        public AcceptBackpackInvite(string userSub, Guid backpackInviteId)
        {
            UserSub = userSub;
            BackpackInviteId = backpackInviteId;
        }
    }
}