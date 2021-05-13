using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class DeleteBackpack : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }

        public DeleteBackpack(string userSub, Guid backpackId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
        }
    }
}