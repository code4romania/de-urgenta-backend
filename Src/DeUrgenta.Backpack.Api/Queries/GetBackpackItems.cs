using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetBackpackItems : IRequest<Result<IImmutableList<BackpackItemModel>, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }

        public GetBackpackItems(string userSub, Guid backpackId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
        }
    }
}
