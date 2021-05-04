using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class InviteToBackpackContributorsHandler : IRequestHandler<InviteToBackpackContributors, Result>
    {
        private readonly IValidateRequest<InviteToBackpackContributors> _validator;
        private readonly DeUrgentaContext _context;

        public InviteToBackpackContributorsHandler(IValidateRequest<InviteToBackpackContributors> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(InviteToBackpackContributors request, CancellationToken cancellationToken)
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