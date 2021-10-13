using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class CreateBackpack : IRequest<Result<BackpackModel, ValidationResult>>
    {
        public string UserSub { get; }
        public BackpackModelRequest Backpack { get; }

        public CreateBackpack(string userSub, BackpackModelRequest backpack)
        {
            UserSub = userSub;
            Backpack = backpack;
        }
    }
}