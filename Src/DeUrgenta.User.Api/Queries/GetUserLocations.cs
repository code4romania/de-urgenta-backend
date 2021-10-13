using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Queries
{
    public class GetUserLocations : IRequest<Result<IImmutableList<UserLocationModel>, ValidationResult>>
    {
        public string UserSub { get; }

        public GetUserLocations(string userSub)
        {
            UserSub = userSub;
        }
    }
}