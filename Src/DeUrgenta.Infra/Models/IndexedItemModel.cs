namespace DeUrgenta.Infra.Models
{
    public sealed record IndexedItemModel
    {
        public int Id { get; init; }
        public string Label { get; init; }
    }
}