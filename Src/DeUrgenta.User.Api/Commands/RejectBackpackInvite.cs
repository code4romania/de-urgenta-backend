using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class RejectBackpackInvite : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid BackpackInviteId { get; }

        public RejectBackpackInvite(string userSub, Guid backpackInviteId)
        {
            UserSub = userSub;
            BackpackInviteId = backpackInviteId;
        }
    }
}