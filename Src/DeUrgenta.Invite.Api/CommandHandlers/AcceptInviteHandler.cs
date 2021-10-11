using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using MediatR;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public class AcceptInviteHandler : IRequestHandler<AcceptInvite, Result<AcceptInviteModel>>
    {
        private readonly IValidateRequest<AcceptInvite> _validator;
        private readonly DeUrgentaContext _context;

        public AcceptInviteHandler(DeUrgentaContext context, IValidateRequest<AcceptInvite> validator)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<AcceptInviteModel>> Handle(AcceptInvite request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<AcceptInviteModel>("Validation failed");
            }



            return new AcceptInviteModel { };
        }
    }
}
