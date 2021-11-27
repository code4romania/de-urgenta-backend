using System;
using DeUrgenta.Domain.Api.Entities;

namespace DeUrgenta.Backpack.Api.Models
{
    public sealed record BackpackItemModel
    {
        public Guid Id { get; init; }
        public Guid BackpackId { get; init; }
        public string Name { get; init; }
        public uint Amount { get; init; }
        public DateTime? ExpirationDate { get; init; }
        public BackpackItemCategoryType Category { get; set; }
        public ulong Version { get; set; }
    }
}