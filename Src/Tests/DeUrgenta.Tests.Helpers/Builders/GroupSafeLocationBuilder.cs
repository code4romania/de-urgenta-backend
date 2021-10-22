using System;
using DeUrgenta.Domain.Api.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class GroupSafeLocationBuilder
    {
        private Group _group;

        public GroupSafeLocation Build() => new()
        {
            Id = Guid.NewGuid(),
            Name = TestDataProviders.RandomString(), 
            Group = _group,
            Latitude = 10.10M,
            Longitude = 11.11M
        };

        public GroupSafeLocationBuilder WithGroup(Group group)
        {
            _group = group;
            return this;
        }
    }
}
