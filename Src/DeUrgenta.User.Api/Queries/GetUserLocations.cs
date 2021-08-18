using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Queries
{
    public class GetUserLocations : IRequest<Result<IImmutableList<UserLocationModel>>>
    {
        public string UserSub { get; }

        public GetUserLocations(string userSub)
        {
            UserSub = userSub;
        }
    }
}