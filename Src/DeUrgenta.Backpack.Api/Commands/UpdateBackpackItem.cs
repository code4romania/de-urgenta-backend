using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class UpdateBackpackItem : IRequest<Result<BackpackItemModel>>
    {
        public Guid ItemId { get; }
        public BackpackItemRequest BackpackItem { get; }
        public UpdateBackpackItem(Guid itemId, BackpackItemRequest backpackItem)
        {
            ItemId = itemId;
            BackpackItem = backpackItem;
        }
    }
}
