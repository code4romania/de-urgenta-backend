using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Specs.Clients;
using DeUrgenta.Specs.Extensions;

namespace DeUrgenta.Specs.Drivers
{
    public class GroupDriver
    {
        public static async Task AddGroupMember(Guid groupId, Client groupOwner, Client target)
        {
            await groupOwner.InviteUserToGroupAsync(groupId, await target.GetUserId());
            var groupInvites= await target.GetGroupInvitesAsync();
            var inviteId = groupInvites.First().InviteId;

            await target.AcceptGroupInviteAsync(inviteId);
        }
    }
}