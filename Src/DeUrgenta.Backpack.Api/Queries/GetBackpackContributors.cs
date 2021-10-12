using System;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetBackpackContributors : IRequest<Result<IImmutableList<BackpackContributorModel>, ValidationResult>>
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