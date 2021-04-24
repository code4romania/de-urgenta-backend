using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetBackpackCategoryItems : IRequest<Result<IImmutableList<BackpackItemModel>>>
    {
        public Guid BackpackId { get; }
        public Guid CategoryId { get; }

        public GetBackpackCategoryItems(Guid backpackId, Guid categoryId)
        {
            BackpackId = backpackId;
            CategoryId = categoryId;
        }

    }
}
