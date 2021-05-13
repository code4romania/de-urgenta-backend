using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class UpdateBackpackHandler : IRequestHandler<UpdateBackpack, Result<BackpackModel>>
    {
        private readonly IValidateRequest<UpdateBackpack> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateBackpackHandler(IValidateRequest<UpdateBackpack> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<BackpackModel>> Handle(UpdateBackpack request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<BackpackModel>("Validation failed");
            }
            var backpack = await _context.Backpacks.FirstAsync(b => b.Id == request.BackpackId, cancellationToken);
            backpack.Name = request.Backpack.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return new BackpackModel
            {
                Id = backpack.Id,
                Name = backpack.Name
            };
        }
    }
}