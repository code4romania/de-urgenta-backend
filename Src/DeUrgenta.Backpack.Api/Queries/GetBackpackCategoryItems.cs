using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Domain.Entities;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetBackpackCategoryItems : IRequest<Result<IImmutableList<BackpackItemModel>>>
    {
        public Guid BackpackId { get; }
        public BackpackCategoryType CategoryId { get; }

        public GetBackpackCategoryItems(Guid backpackId, BackpackCategoryType categoryId)
        {
            BackpackId = backpackId;
            CategoryId = categoryId;
        }

    }
}
