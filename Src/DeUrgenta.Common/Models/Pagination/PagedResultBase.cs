namespace DeUrgenta.Common.Models.Pagination
{
    public abstract record PagedResultBase
    {
        public int CurrentPage { get; init; }
        public int PageCount { get; init; }
        public int PageSize { get; init; }
        public int RowCount { get; init; }
    }
}