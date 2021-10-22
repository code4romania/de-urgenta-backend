using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Options;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class UpdateBackpackHandler : IRequestHandler<UpdateBackpack, Result<BackpackModel, ValidationResult>>
    {
        private readonly IValidateRequest<UpdateBackpack> _validator;
        private readonly DeUrgentaContext _context;
        private readonly BackpacksConfig _config;

        public UpdateBackpackHandler(IValidateRequest<UpdateBackpack> validator, DeUrgentaContext context, IOptions<BackpacksConfig> config)
        {
            _validator = validator;
            _context = context;
            _config = config.Value;
        }

        public async Task<Result<BackpackModel, ValidationResult>> Handle(UpdateBackpack request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }
            var backpack = await _context.Backpacks.FirstAsync(b => b.Id == request.BackpackId, cancellationToken);
            backpack.Name = request.Backpack.Name;

            await _context.SaveChangesAsync(cancellationToken);

            var contributorsCount = await _context.BackpacksToUsers.CountAsync(b => b.BackpackId == request.BackpackId, cancellationToken);

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