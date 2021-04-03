using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Common.Models
{
    public sealed record PaginationQueryModel
    {
        [FromQuery(Name = "pageNumber")] public int PageNumber { get; init; } = 1;
        [FromQuery(Name = "pageSize")] public int PageSize { get; init; } = 25;
    }
}