using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class DeleteBackpackHandler : IRequestHandler<DeleteBackpack, Result>
    {
        private readonly IValidateRequest<DeleteBackpack> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteBackpackHandler(IValidateRequest<DeleteBackpack> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(DeleteBackpack request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            return Result.Success();
        }
    }
}