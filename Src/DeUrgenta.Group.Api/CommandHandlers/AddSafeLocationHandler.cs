using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class AddSafeLocationHandler : IRequestHandler<AddSafeLocation, Result<SafeLocationModel>>
    {
        private readonly IValidateRequest<AddSafeLocation> _validator;
        private readonly DeUrgentaContext _context;

        public AddSafeLocationHandler(IValidateRequest<AddSafeLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<SafeLocationModel>> Handle(AddSafeLocation request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<SafeLocationModel>("Validation failed");
            }

            return new SafeLocationModel();
        }
    }
}