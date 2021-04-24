using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetBackpackItems : IRequest<Result<IImmutableList<BackpackItemModel>>>
    {
        public Guid BackpackId { get; }

        public GetBackpackItems(Guid backpackId)
        {
            BackpackId = backpackId;
        }
    }
}
