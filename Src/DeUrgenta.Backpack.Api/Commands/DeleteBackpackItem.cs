using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class DeleteBackpackItem : IRequest<Result>
    {
        public Guid ItemId { get; }
        public DeleteBackpackItem(Guid itemId)
        {
            ItemId = itemId;
        }
    }
}
