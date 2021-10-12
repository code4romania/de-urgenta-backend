using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Queries
{
    public class GetGroupInvites : IRequest<Result<IImmutableList<GroupInviteModel>, ValidationResult>>
    {
        public string UserSub { get; }

        public GetGroupInvites(string userSub)
        {
            UserSub = userSub;
        }
    }
}