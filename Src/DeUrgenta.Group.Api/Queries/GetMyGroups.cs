using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Queries
{
    public class GetMyGroups : IRequest<Result<IImmutableList<GroupModel>>>
    {
        public string UserSub { get; }

        public GetMyGroups(string userSub)
        {
            UserSub = userSub;
        }
    }
}