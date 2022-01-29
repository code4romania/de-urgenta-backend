using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class DeleteBackpackItem : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid ItemId { get; }
        public DeleteBackpackItem(string userSub, Guid itemId)
        {
            UserSub = userSub;
            ItemId = itemId;
        }
    }
}
