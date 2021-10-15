using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Tests.Helpers.Builders
{
    public class BackpackItemBuilder
    {
        private DateTime? _expirationDate = null;
        private readonly Backpack _backpack = new BackpackBuilder().Build();
        private BackpackCategoryType _category = BackpackCategoryType.FirstAid;

        public BackpackItem Build() => new()
        {
            Id = Guid.NewGuid(),
            Amount = 2,
            ExpirationDate = _expirationDate,
            Name = TestDataProviders.RandomString(),
            BackpackCategory = _category,
            BackpackId = Guid.NewGuid(),
            Backpack = _backpack
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
    }
}
