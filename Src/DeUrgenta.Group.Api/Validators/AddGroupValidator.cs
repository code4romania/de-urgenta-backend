﻿using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Group.Api.Validators
{
    public class AddGroupValidator : IValidateRequest<AddGroup>
    {
        private readonly DeUrgentaContext _context;
        private readonly GroupsConfig _groupsConfig;

        public AddGroupValidator(DeUrgentaContext context, GroupsConfig groupsConfig)
        {
            _context = context;
            _groupsConfig = groupsConfig;
        }
        
        public AddGroupValidator(DeUrgentaContext context, IOptions<GroupsConfig> groupsConfig)
        {
            _context = context;
            _groupsConfig = groupsConfig.Value;
        }

        public async Task<bool> IsValidAsync(AddGroup request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return false;
            }
            
            if (user.GroupsAdministered.Count >= _groupsConfig.MaxCreatedGroupsPerUser)
            {
                return false;
            }

            return true;
        }
    }
}