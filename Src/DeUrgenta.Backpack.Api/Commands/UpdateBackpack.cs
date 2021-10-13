using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class UpdateBackpack : IRequest<Result<BackpackModel, ValidationResult>>
    {
        public string UserSub { get; }
        public Guid BackpackId { get; }
        public BackpackModelRequest Backpack { get; }

        public UpdateBackpack(string userSub, Guid backpackId, BackpackModelRequest backpack)
        {
            UserSub = userSub;
            BackpackId = backpackId;
            Backpack = backpack;
        }
    }
}