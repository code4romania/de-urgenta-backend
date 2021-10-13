using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class RemoveCurrentUserFromContributors : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }

        public RemoveCurrentUserFromContributors(string userSub, Guid backpackId)
        {
            UserSub = userSub;
            BackpackId = backpackId;
        }
    }
}