using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class UserBuilder
    {
        private Guid _userId = Guid.NewGuid();
        private string _sub = Guid.NewGuid().ToString();

        public User Build() => new()
        {
            Id = _userId,
            FirstName = TestDataProviders.RandomString(),
            LastName = TestDataProviders.RandomString(),
            Sub = _sub,
            Email = $"{_userId}@example.com"
        };

        public UserBuilder WithId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public UserBuilder WithSub(string contributorSub)
        {
            _sub = contributorSub;
            return this;
        }
    }
}
