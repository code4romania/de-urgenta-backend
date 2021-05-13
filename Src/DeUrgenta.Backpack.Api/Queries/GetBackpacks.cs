using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetBackpacks : IRequest<Result<IImmutableList<BackpackModel>>>
    {
        public string UserSub { get; }

        public GetBackpacks(string userSub)
        {
            UserSub = userSub;
        }
    }
}