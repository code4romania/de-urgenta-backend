using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Backpack.Api.CommandHandlers
{
    public class RemoveContributorHandler : IRequestHandler<RemoveContributor, Result>
    {
        private readonly IValidateRequest<RemoveContributor> _validator;
        private readonly DeUrgentaContext _context;

        public RemoveContributorHandler(IValidateRequest<RemoveContributor> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(RemoveContributor request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var backpackToUser = await _context
                .BackpacksToUsers
                .FirstAsync(btu => btu.User.Id == request.UserId && btu.Backpack.Id == request.BackpackId, cancellationToken);
            _context.BackpacksToUsers.Remove(backpackToUser);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}