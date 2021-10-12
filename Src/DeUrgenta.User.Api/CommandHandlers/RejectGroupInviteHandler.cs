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
    public class RejectGroupInviteHandler : IRequestHandler<RejectGroupInvite, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<RejectGroupInvite> _validator;
        private readonly DeUrgentaContext _context;

        public RejectGroupInviteHandler(IValidateRequest<RejectGroupInvite> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(RejectGroupInvite request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var invite = await _context
                .GroupInvites
                .FirstAsync(gi => gi.Id == request.GroupInviteId, cancellationToken);

            _context.GroupInvites.Remove(invite);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}