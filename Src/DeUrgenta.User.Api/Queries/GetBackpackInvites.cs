using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Queries
{
    public class GetBackpackInvites : IRequest<Result<IImmutableList<BackpackInviteModel>, ValidationResult>>
    {
        public string UserSub { get; }

        public GetBackpackInvites(string userSub)
        {
            UserSub = userSub;
        }
    }
}