using System;
using DeUrgenta.Domain.Api.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class UserBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _sub = Guid.NewGuid().ToString();

        public User Build() => new()
        {
            Id = _id,
            FirstName = TestDataProviders.RandomString(),
            LastName = TestDataProviders.RandomString(),
            Sub = _sub,
            Email = $"{_id}@example.com"
        };

        public UserBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public UserBuilder WithSub(string contributorSub)
        {
            _sub = contributorSub;
            return this;
        }
    }
}
