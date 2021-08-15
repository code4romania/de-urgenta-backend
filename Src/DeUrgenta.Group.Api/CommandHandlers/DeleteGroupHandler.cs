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
    public class DeleteGroupHandler : IRequestHandler<DeleteGroup, Result>
    {
        private readonly IValidateRequest<DeleteGroup> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteGroupHandler(IValidateRequest<DeleteGroup> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(DeleteGroup request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var group = await _context.Groups
                .Include(x=>x.Backpack)
                .FirstAsync(g => g.AdminId == user.Id, cancellationToken);

            _context.Backpacks.Remove(group.Backpack);
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}