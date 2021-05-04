using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class RemoveCurrentUserFromContributorsHandler : IRequestHandler<RemoveCurrentUserFromContributors, Result>
    {
        private readonly IValidateRequest<RemoveCurrentUserFromContributors> _validator;
        private readonly DeUrgentaContext _context;

        public RemoveCurrentUserFromContributorsHandler(IValidateRequest<RemoveCurrentUserFromContributors> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(RemoveCurrentUserFromContributors request, CancellationToken cancellationToken)
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