using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class DeleteBackpackItem : IRequest<Result>
    {
        public string UserSub { get; }
        public Guid ItemId { get; }
        public DeleteBackpackItem(string userSub, Guid itemId)
        {
            UserSub = userSub;
            ItemId = itemId;
        }
    }
}
