﻿using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Invite.Api.Validators
{
    public class AcceptBackpackInviteValidator : IValidateRequest<AcceptBackpackInvite>
    {
        private readonly DeUrgentaContext _context;
        private readonly BackpacksConfig _config;

        public AcceptBackpackInviteValidator(DeUrgentaContext context, IOptions<BackpacksConfig> config)
        {
            _context = context;
            _config = config.Value;
        }

        public async Task<ValidationResult> IsValidAsync(AcceptBackpackInvite request)
        {
            var backpack = await _context.Backpacks.FirstOrDefaultAsync(b => b.Id == request.BackpackId);
            if (backpack == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub);

            var userIsAlreadyAContributor = await _context.BackpacksToUsers
                .AnyAsync(u => u.UserId == user.Id && u.BackpackId == request.BackpackId);

            if (userIsAlreadyAContributor)
            {
                return new LocalizableValidationError("cannot-accept-invite", "already-backpack-contributor");
            }

            var existingContributors = await _context.BackpacksToUsers.CountAsync(b => b.BackpackId == request.BackpackId);
            if (existingContributors >= _config.MaxContributors)
            {
                return new LocalizableValidationError("cannot-accept-invite", "max-backpack-contributors-reached");
            }

            return ValidationResult.Ok;
        }

    }
}