using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class InviteBuilder
    {
        private Guid _id = Guid.NewGuid();
        private Guid _destinationId = Guid.NewGuid();
        private InviteStatus _status = InviteStatus.Sent;
        private InviteType _type = InviteType.Backpack;

        public Invite Build() => new()
        {
            Id = _id,
            DestinationId = _destinationId,
            InviteStatus = _status,
            SentOn = DateTime.Today,
            Type = _type
        };

        public InviteBuilder WithStatus(InviteStatus status)
        {
            _status = status;
            return this;
        }

        public InviteBuilder WithId(Guid inviteId)
        {
            _id = inviteId;
            return this;
        }

        public InviteBuilder WithType(InviteType inviteType)
        {
            _type = inviteType;
            return this;
        }

        public InviteBuilder WithDestinationId(Guid destinationId)
        {
            _destinationId = destinationId;
            return this;
        }
    }
}
