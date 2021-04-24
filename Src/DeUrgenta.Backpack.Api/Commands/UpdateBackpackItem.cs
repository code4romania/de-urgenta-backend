using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class UpdateBackpackItem : IRequest<Result<BackpackItemModel>>
    {
        public Guid BackpackId { get; set; }
        public BackpackItemRequest BackpackItem { get; set; }
        public UpdateBackpackItem(Guid backpackId, BackpackItemRequest backpackItem)
        {
            BackpackId = backpackId;
            BackpackItem = backpackItem;
        }
    }
}
