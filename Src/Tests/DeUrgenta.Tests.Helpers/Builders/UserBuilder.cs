using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class UserBuilder
    {
        private Guid _userId = Guid.NewGuid();

        public User Build() => new()
        {
            Id = _userId,
            FirstName = TestDataProviders.RandomString(),
            LastName = TestDataProviders.RandomString(),
            Sub = Guid.NewGuid().ToString()
        };

        public UserBuilder WithId(Guid userId)
        {
            _userId = userId;
            return this;
        }
    }
}
