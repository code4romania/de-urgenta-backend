using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Invite.Api.Models;
using MediatR;

namespace DeUrgenta.Invite.Api.Commands
{
    public class AcceptBackpackInvite : IRequest<Result<AcceptInviteModel, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }

        public AcceptBackpackInvite(string userSub, Guid backpackId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
        }
    }
}