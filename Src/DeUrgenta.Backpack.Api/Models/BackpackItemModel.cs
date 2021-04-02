using System;
using DeUrgenta.Domain.Entities;

namespace DeUrgenta.Backpack.Api.Models
{
    public sealed record BackpackItemModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public uint Amount { get; init; }
        public DateTime ExpirationDate { get; init; }
        public BackpackCategoryType CategoryType { get; set; }
    }
}