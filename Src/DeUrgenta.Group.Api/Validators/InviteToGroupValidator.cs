﻿using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Group.Api.Validators
{
    public class InviteToGroupValidator : IValidateRequest<InviteToGroup>
    {
        private readonly DeUrgentaContext _context;
        private readonly GroupsConfig _groupsConfig;

        public InviteToGroupValidator(DeUrgentaContext context, GroupsConfig groupsConfig)
        {
            _context = context;
            _groupsConfig = groupsConfig;
        }
        
        public InviteToGroupValidator(DeUrgentaContext context, IOptions<GroupsConfig> groupsConfig)
        {
            _context = context;
            _groupsConfig = groupsConfig.Value;
        }

        public async Task<bool> IsValidAsync(InviteToGroup request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            var invitedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null || invitedUser == null)
            {
                return false;
            }

            if (user.Id == invitedUser.Id)
            {
                return false;
            }

            var isPartOfGroup = await _context.UsersToGroups.AnyAsync(utg =>
                     utg.User.Id == user.Id
                     && utg.Group.Id == request.GroupId);

            if (!isPartOfGroup)
            {
                return false;
            }

            var isAlreadyInvited = await _context.GroupInvites.AnyAsync(utg =>
                utg.InvitationReceiver.Id == invitedUser.Id
                && utg.Group.Id == request.GroupId);

            if (isAlreadyInvited)
            {
                return false;
            }

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == request.GroupId);
            if (group == null)
            {
                return false;
            }
            
            if (invitedUser.GroupsMember.Count >= _groupsConfig.MaxJoinedGroupsPerUser)
            {
                return false;
            }

            return true;
        }
    }
}