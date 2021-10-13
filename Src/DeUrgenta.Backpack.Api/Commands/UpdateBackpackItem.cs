using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class UpdateBackpackItem : IRequest<Result<BackpackItemModel, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid ItemId { get; }
        public BackpackItemRequest BackpackItem { get; }
        public UpdateBackpackItem(string userSub, Guid itemId, BackpackItemRequest backpackItem)
        {
            UserSub = userSub;
            ItemId = itemId;
            BackpackItem = backpackItem;
        }
    }
}
