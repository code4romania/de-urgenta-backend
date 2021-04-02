using System;

namespace DeUrgenta.Backpack.Api.Models
{
    public sealed record BackpackCategoryModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
    }
}