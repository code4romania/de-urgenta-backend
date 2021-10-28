using System;
using DeUrgenta.Domain.Api.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class BackpackBuilder
    {
        private Guid _id = Guid.NewGuid();
        
        public Backpack Build() => new()
        {
            Id = _id,
            Name = TestDataProviders.RandomString()
        };

        public BackpackBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
    }
}