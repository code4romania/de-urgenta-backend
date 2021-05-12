using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetBackpackContributors : IRequest<Result<IImmutableList<BackpackContributorModel>>>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }

        public GetBackpackContributors(string userSub, Guid backpackId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
        }
    }
}