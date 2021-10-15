using System;
using DeUrgenta.Domain.Api.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class BackpackBuilder
    {
        private Guid _id = Guid.NewGuid();

        public Backpack Build() => new()
        {
            Name = TestDataProviders.RandomString(),
            Id = _id
        };

        public BackpackBuilder WithId(Guid backpackId)
        {
            _id = backpackId;
            return this;
        }
    }
}