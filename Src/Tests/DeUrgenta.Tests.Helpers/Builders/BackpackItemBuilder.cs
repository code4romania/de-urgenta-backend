using System;
using DeUrgenta.Domain.Api.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class BackpackItemBuilder
    {
        private Guid _id = Guid.NewGuid();
        private DateTime? _expirationDate;
        private Backpack _backpack = new BackpackBuilder().Build();
        private BackpackCategoryType _category = BackpackCategoryType.FirstAid;
        private ulong _version;
        
        public BackpackItem Build() => new()
        {
            Id = _id,
            Amount = 2,
            ExpirationDate = _expirationDate,
            Name = TestDataProviders.RandomString(),
            BackpackCategory = _category,
            BackpackId = Guid.NewGuid(),
            Backpack = _backpack,
            Version = _version
        };

        public BackpackItemBuilder WithExpirationDate(DateTime? expirationDate)
        {
            _expirationDate = expirationDate;
            return this;
        }

        public BackpackItemBuilder WithCategory(BackpackCategoryType backpackCategoryType)
        {
            _category = backpackCategoryType;
            return this;
        }
        
        public BackpackItemBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
        
        public BackpackItemBuilder WithBackpack(Backpack backpack)
        {
            _backpack = backpack;
            return this;
        }

        public BackpackItemBuilder WithVersion(ulong version)
        {
            _version = version;
            return this;
        }
    }
}
