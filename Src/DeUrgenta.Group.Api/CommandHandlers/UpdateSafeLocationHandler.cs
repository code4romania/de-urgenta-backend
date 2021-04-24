using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Validators;
using MediatR;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class UpdateSafeLocationHandler : IRequestHandler<UpdateSafeLocation, Result<SafeLocationModel>>
    {
        private readonly IValidateRequest<UpdateSafeLocation> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateSafeLocationHandler(IValidateRequest<UpdateSafeLocation> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result<SafeLocationModel>> Handle(UpdateSafeLocation request, CancellationToken cancellationToken)
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