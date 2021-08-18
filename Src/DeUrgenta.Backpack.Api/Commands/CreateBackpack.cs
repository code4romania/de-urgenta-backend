using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using MediatR;

namespace DeUrgenta.Backpack.Api.Commands
{
    public class CreateBackpack : IRequest<Result<BackpackModel>>
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