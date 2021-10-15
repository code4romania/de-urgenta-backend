﻿using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Invite.Api.Validators
{
    public class CreateGroupInviteValidator : ICreateInviteValidator
    {
        private readonly DeUrgentaContext _context;
        private readonly GroupsConfig _config;

        public CreateGroupInviteValidator(DeUrgentaContext context, IOptions<GroupsConfig> config)
        {
            _context = context;
            _config = config.Value;
        }

        public async Task<ValidationResult> ValidateAsync(CreateInvite request)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == request.DestinationId);
            if (group == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var userIsGroupMember = await _context.UsersToGroups
                .AnyAsync(u => u.UserId == request.UserId
                                     && u.GroupId == request.DestinationId);
            if (!userIsGroupMember)
            {
                return ValidationResult.GenericValidationError;
            }

            if (group.GroupMembers.Count == _config.UsersLimit)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}
