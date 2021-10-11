using System;
using DeUrgenta.Invite.Api.Models;

namespace DeUrgenta.Invite.Api.Validators
{
    public class InviteValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public InviteValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICreateInviteValidator GetCreateValidatorInstance(InviteType inviteType)
        {
            return inviteType switch
            {
                InviteType.Group => (ICreateInviteValidator)_serviceProvider.GetService(typeof(CreateGroupInviteValidator)),
                InviteType.Backpack => (ICreateInviteValidator)_serviceProvider.GetService(typeof(CreateBackpackInviteValidator)),
                _ => throw new ArgumentException(nameof(inviteType))
            };
        }

        public IAcceptInviteValidator GetAcceptValidatorInstance(InviteType inviteType)
        {
            return inviteType switch
            {
                InviteType.Group => (IAcceptInviteValidator)_serviceProvider.GetService(typeof(AcceptGroupInviteValidator)),
                InviteType.Backpack => (IAcceptInviteValidator)_serviceProvider.GetService(typeof(AcceptBackpackInviteValidator)),
                _ => throw new ArgumentException(nameof(inviteType))
            };
        }
    }
}