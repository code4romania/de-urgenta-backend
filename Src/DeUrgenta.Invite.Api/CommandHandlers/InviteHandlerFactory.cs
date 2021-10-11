using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public class InviteHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public InviteHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IInviteHandler GetHandlerInstance(InviteType inviteType)
        {
            return inviteType switch
            {
                InviteType.Group => (IInviteHandler)_serviceProvider.GetService(typeof(AcceptGroupInviteHandler)),
                InviteType.Backpack => (IInviteHandler)_serviceProvider.GetService(typeof(AcceptBackpackInviteHandler)),
                _ => throw new ArgumentException(nameof(inviteType))
            };
        }
    }
}
