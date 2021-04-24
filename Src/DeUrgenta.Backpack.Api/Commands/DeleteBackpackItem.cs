using System;
using CSharpFunctionalExtensions;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class DeleteBackpackItem : IRequest<Result>
    {
        public Guid BackpackId { get; set; }
        public DeleteBackpackItem(Guid backpackId)
        {
            BackpackId = backpackId;
        }
    }
}
