using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class CreateBackpackHandler : IRequestHandler<CreateBackpack, Result<BackpackModel>>
    {
        private readonly IValidateRequest<CreateBackpack> _validator;
        private readonly DeUrgentaContext _context;

        public CreateBackpackHandler(IValidateRequest<CreateBackpack> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<BackpackModel>> Handle(CreateBackpack request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return Result.Failure<BackpackModel>("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);

            var backpack = new Domain.Entities.Backpack { Name = request.Backpack.Name };
            var backpackToUser = new BackpackToUser { Backpack = backpack, User = user, IsOwner = true };

            await _context.Backpacks.AddAsync(backpack, cancellationToken);
            await _context.BackpacksToUsers.AddAsync(backpackToUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new BackpackModel
            {
                Id = backpack.Id,
                Name = backpack.Name
            };
        }
    }
}