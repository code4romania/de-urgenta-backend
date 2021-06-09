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
    public class GetGroupInvitesHandler : IRequestHandler<GetGroupInvites, Result<IImmutableList<GropInviteModel>>>
    {
        private readonly IValidateRequest<GetGroupInvites> _validator;
        private readonly DeUrgentaContext _context;

        public GetGroupInvitesHandler(IValidateRequest<GetGroupInvites> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<GropInviteModel>>> Handle(GetGroupInvites request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<GropInviteModel>>("Validation failed");
            }

            var groupInvites = await _context
                .GroupInvites
                .Where(gi => gi.InvitationReceiver.Sub == request.UserSub)
                .Select(x => new GropInviteModel
                {
                    SenderId = x.InvitationSenderId,
                    GroupId = x.GroupId,
                    GroupName = x.Group.Name,
                    InviteId = x.Id,
                    SenderFirstName = x.InvitationSender.FirstName,
                    SenderLastName = x.InvitationSender.LastName
                })
                .ToListAsync(cancellationToken);

            return groupInvites.ToImmutableList();
        }
    }
}