using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Entities;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetBackpackCategoryItems : IRequest<Result<IImmutableList<BackpackItemModel>, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }
        public BackpackCategoryType CategoryId { get; }

        public GetBackpackCategoryItems(string userSub, Guid backpackId, BackpackCategoryType categoryId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
            CategoryId = categoryId;
        }

    }
}
