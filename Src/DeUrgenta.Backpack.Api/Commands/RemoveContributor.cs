using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class RemoveContributor : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }
        public Guid UserId { get; }

        public RemoveContributor(string userSub, Guid backpackId, Guid userId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
            UserId = userId;
        }
    }
}