using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class InviteToBackpackContributors : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }
        public Guid UserId { get; }

        public InviteToBackpackContributors(string userSub, Guid backpackId, Guid userId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
            UserId = userId;
        }
    }
}