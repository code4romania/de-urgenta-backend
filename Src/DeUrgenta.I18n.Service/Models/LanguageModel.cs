using System;

namespace DeUrgenta.I18n.Service.Models
{
    public sealed record LanguageModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Culture { get; init; }
    }
}