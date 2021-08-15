using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Queries
{
    public class GetBackpackInvites : IRequest<Result<IImmutableList<BackpackInviteModel>>>
    {
        public string UserSub { get; }

        public GetBackpackInvites(string userSub)
        {
            UserSub = userSub;
        }
    }
}