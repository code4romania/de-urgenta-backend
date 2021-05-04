using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;

namespace DeUrgenta.Backpack.Api.QueryHandlers
{
    public class GetMyBackpacksHandler : IRequestHandler<GetMyBackpacks, Result<IImmutableList<BackpackModel>>>
    {
        private readonly IValidateRequest<GetMyBackpacks> _validator;
        private readonly DeUrgentaContext _context;

        public GetMyBackpacksHandler(IValidateRequest<GetMyBackpacks> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<BackpackModel>>> Handle(GetMyBackpacks request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<BackpackModel>>("Validation failed");
            }

            return ImmutableList<BackpackModel>.Empty;
        }
    }
}