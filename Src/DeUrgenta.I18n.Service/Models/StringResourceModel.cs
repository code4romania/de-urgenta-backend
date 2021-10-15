using System;

namespace DeUrgenta.I18n.Service.Models
{
    public sealed record StringResourceModel
    {
        public Guid Id { get; init; }
        public string Key { get; init; }
        public string Value { get; init; }
    }
}