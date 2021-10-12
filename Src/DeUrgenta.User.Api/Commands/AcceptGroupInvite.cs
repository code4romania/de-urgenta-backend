using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class AcceptGroupInvite : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid GroupInviteId { get; }

        public AcceptGroupInvite(string userSub, Guid groupInviteId)
        {
            UserSub = userSub;
            GroupInviteId = groupInviteId;
        }
    }
}