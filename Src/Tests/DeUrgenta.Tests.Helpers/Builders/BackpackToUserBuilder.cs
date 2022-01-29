using System;
using DeUrgenta.Domain.Api.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class BackpackToUserBuilder
    {
        private Guid _backpackId = Guid.NewGuid();
        private Guid _userId = Guid.NewGuid();
        private Backpack _backpack = new BackpackBuilder().Build();
        private User _user = new UserBuilder().Build();

        public BackpackToUser Build() => new()
        {
            BackpackId = _backpackId,
            Backpack = _backpack,
            UserId = _userId,
            User = _user
        };

        public BackpackToUserBuilder WithBackpack(Backpack backpack)
        {
            _backpackId = backpack.Id;
            _backpack = backpack;
            return this;
        }

        public BackpackToUserBuilder WithUser(User user)
        {
            _userId = user.Id;
            _user = user;
            return this;
        }
    }
}
