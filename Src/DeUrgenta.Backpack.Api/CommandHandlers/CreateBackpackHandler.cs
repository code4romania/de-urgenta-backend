using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Options;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class CreateBackpackHandler : IRequestHandler<CreateBackpack, Result<BackpackModel, ValidationResult>>
    {
        private readonly IValidateRequest<CreateBackpack> _validator;
        private readonly DeUrgentaContext _context;
        private readonly BackpacksConfig _config;

        public CreateBackpackHandler(IValidateRequest<CreateBackpack> validator, DeUrgentaContext context, IOptions<BackpacksConfig> config)
        {
            _validator = validator;
            _context = context;
            _config = config.Value;
        }

        public async Task<Result<BackpackModel, ValidationResult>> Handle(CreateBackpack request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);

            var backpack = new Domain.Api.Entities.Backpack { Name = request.Backpack.Name };
            var backpackToUser = new BackpackToUser { Backpack = backpack, User = user, IsOwner = true };
            
            await _context.Backpacks.AddAsync(backpack, cancellationToken);
            await _context.BackpacksToUsers.AddAsync(backpackToUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var contributorsCount = await _context.BackpacksToUsers.CountAsync(b => b.BackpackId == backpack.Id, cancellationToken);

            return new BackpackModel
            {
                Id = backpack.Id,
                Name = backpack.Name,
                NumberOfContributors = contributorsCount,
                MaxNumberOfContributors = _config.MaxContributors
            };
        }
    }
}