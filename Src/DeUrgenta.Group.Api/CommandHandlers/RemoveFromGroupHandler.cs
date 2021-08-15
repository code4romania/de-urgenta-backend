﻿using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class RemoveFromGroupHandler : IRequestHandler<RemoveFromGroup, Result>
    {
        private readonly IValidateRequest<RemoveFromGroup> _validator;
        private readonly DeUrgentaContext _context;

        public RemoveFromGroupHandler(IValidateRequest<RemoveFromGroup> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(RemoveFromGroup request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var groupAssignment = await _context
                .UsersToGroups
                .Include(utg=>utg.Group)
                .ThenInclude(utg=>utg.Backpack)
                .FirstAsync(utg => utg.Group.Id == request.GroupId && utg.User.Id == request.UserId, cancellationToken);

            var backpackAssignment = await _context
                .BackpacksToUsers
                .FirstAsync(btu => btu.Backpack.Id == groupAssignment.Group.Backpack.Id && btu.User.Id == request.UserId, cancellationToken);

            _context.BackpacksToUsers.Remove(backpackAssignment);
            _context.UsersToGroups.Remove(groupAssignment);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}