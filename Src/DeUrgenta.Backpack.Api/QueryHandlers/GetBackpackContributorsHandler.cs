using System;
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
    public class GetBackpackContributorsHandler : IRequestHandler<GetBackpackContributors,Result<IImmutableList<BackpackContributorModel>>>
    {
        private readonly IValidateRequest<GetBackpackContributors> _validator;
        private readonly DeUrgentaContext _context;

        public GetBackpackContributorsHandler(IValidateRequest<GetBackpackContributors> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<BackpackContributorModel>>> Handle(GetBackpackContributors request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure< IImmutableList<BackpackContributorModel>>("Validation failed");
            }

            return ImmutableList<BackpackContributorModel>.Empty;
        }
    }
}