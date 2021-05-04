using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;

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
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<BackpackModel>("Validation failed");
            }

            return null;
        }
    }
}