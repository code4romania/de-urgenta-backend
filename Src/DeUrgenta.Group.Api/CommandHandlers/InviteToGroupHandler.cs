using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using DeUrgenta.Group.Api.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class InviteToGroupHandler : IRequestHandler<InviteToGroup, Result>
    {
        private readonly IValidateRequest<InviteToGroup> _validator;
        private readonly DeUrgentaContext _context;

        public InviteToGroupHandler(IValidateRequest<InviteToGroup> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(InviteToGroup request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var group = await _context.Groups.FirstAsync(g => g.Id == request.GroupId, cancellationToken);

            var invitedUser = await _context.Users.FirstAsync(u => u.Id == request.UserId, cancellationToken);

            _context.GroupInvites.Add(new GroupInvite
            {
                Group = group,
                InvitationReceiver = invitedUser,
                InvitationSender = user
            });

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}