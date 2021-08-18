using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.QueryHandlers
{
    public class GetBackpackInvitesHandler : IRequestHandler<GetBackpackInvites, Result<IImmutableList<BackpackInviteModel>>>
    {
        private readonly IValidateRequest<GetBackpackInvites> _validator;
        private readonly DeUrgentaContext _context;

        public GetBackpackInvitesHandler(IValidateRequest<GetBackpackInvites> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<BackpackInviteModel>>> Handle(GetBackpackInvites request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<BackpackInviteModel>>("Validation failed");
            }

            var groupInvites = await _context
                .BackpackInvites
                .Where(gi => gi.InvitationReceiver.Sub == request.UserSub)
                .Select(x => new BackpackInviteModel
                {
                    SenderId = x.InvitationSenderId,
                    BackpackId = x.BackpackId,
                    BackpackName = x.Backpack.Name,
                    InviteId = x.Id,
                    SenderFirstName = x.InvitationSender.FirstName,
                    SenderLastName = x.InvitationSender.LastName
                })
                .ToListAsync(cancellationToken);

            return groupInvites.ToImmutableList();
        }
    }
}