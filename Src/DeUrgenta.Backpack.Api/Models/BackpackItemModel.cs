using System;

namespace DeUrgenta.Backpack.Api.Models
{
    public sealed record BackpackItemModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public uint Amount { get; init; }
        public DateTime ExpirationDate { get; init; }
    }
}