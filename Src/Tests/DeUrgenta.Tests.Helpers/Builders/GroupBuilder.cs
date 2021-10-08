using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class GroupBuilder
    {
        private User _admin = new UserBuilder().Build();
        private Guid _id = Guid.NewGuid();

        public Group Build() => new()
        {
            Admin = _admin, 
            Name = TestDataProviders.RandomString(),
            Id = _id
        };

        public GroupBuilder WithAdmin(User admin)
        {
            _admin = admin;
            return this;
        }

        public GroupBuilder WithId(Guid groupId)
        {
            _id = groupId;
            return this;
        }
    }
}