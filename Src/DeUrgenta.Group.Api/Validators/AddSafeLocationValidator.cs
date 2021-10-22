using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Group.Api.Validators
{
    public class AddSafeLocationValidator : IValidateRequest<AddSafeLocation>
    {
        private readonly DeUrgentaContext _context;
        private readonly GroupsConfig _config;

        public AddSafeLocationValidator(DeUrgentaContext context, IOptions<GroupsConfig> config)
        {
            _context = context;
            _config = config.Value;
        }

        public async Task<ValidationResult> IsValidAsync(AddSafeLocation request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId);
            if (group == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var isGroupAdmin = group.Admin.Id == user.Id;
            if (!isGroupAdmin)
            {
                return new DetailedValidationError("Cannot add safe locations", "Only group admins can add safe locations.");
            }

            var groupHasMaxSafeLocations = group.GroupSafeLocations.Count >= _config.MaxSafeLocations;
            if (groupHasMaxSafeLocations)
            {
                return new DetailedValidationError("Cannot add more safe locations", "You have reached maximum number of safe locations per group.");
            }

            return ValidationResult.Ok;
        }
    }
}