using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Backpack.Api.Queries
{
    public class GetMyBackpacks : IRequest<Result<IImmutableList<BackpackModel>, ValidationResult>>
    {
        public string UserSub { get; }

        public GetMyBackpacks(string userSub)
        {
            UserSub = userSub;
        }
    }
}