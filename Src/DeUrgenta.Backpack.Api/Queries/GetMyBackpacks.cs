using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetMyBackpacks : IRequest<Result<IImmutableList<BackpackModel>>>
    {
        public string UserSub { get; }

        public GetMyBackpacks(string userSub)
        {
            UserSub = userSub;
        }
    }
}