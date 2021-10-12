using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class UserToGroupBuilder
    {
        private Guid _groupId = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();
        private Group _group = new GroupBuilder().Build();
        private User _user = new UserBuilder().Build();

        public UserToGroup Build() => new()
        {
            GroupId = _groupId,
            Group = _group,
            UserId = _userId,
            User = _user
        };

        public UserToGroupBuilder WithGroupId(Guid groupId)
        {
            _groupId = groupId;
            return this;
        }

        public UserToGroupBuilder WithGroup(Group group)
        {
            _group = group;
            _groupId = group.Id;
            return this;
        }

        public UserToGroupBuilder WithUser(User user)
        {
            _user = user;
            _userId = user.Id;
            return this;
        }
    }
}