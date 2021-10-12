using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.CommandHandlers
{
    public class RejectBackpackInviteHandler : IRequestHandler<RejectBackpackInvite, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<RejectBackpackInvite> _validator;
        private readonly DeUrgentaContext _context;

        public RejectBackpackInviteHandler(IValidateRequest<RejectBackpackInvite> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(RejectBackpackInvite request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var invite = await _context
                .BackpackInvites
                .FirstAsync(bi => bi.Id == request.BackpackInviteId, cancellationToken);

            _context.BackpackInvites.Remove(invite);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}