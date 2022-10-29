﻿using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace DeUrgenta.Backpack.Api.Validators
{
    public class AddBackpackItemValidator : IValidateRequest<AddBackpackItem>
    {
        private readonly DeUrgentaContext _context;
        public AddBackpackItemValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(AddBackpackItem request, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub, ct);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isContributor = await _context.BackpacksToUsers.AnyAsync(btu => btu.User.Id == user.Id 
                && btu.Backpack.Id == request.BackpackId, ct);

            if (!isContributor)
            {
                return ValidationResult.GenericValidationError;
            }

            return ValidationResult.Ok;
        }
    }
}
