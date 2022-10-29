using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Common.Extensions
{
    public static class QueryExtensions
    {
        // NB! Don’t change IQueryable to IEnumerable because otherwise regular Count() method of LINQ is called instead of Entity Framework one.
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query,
            int? page, int? pageSize, CancellationToken ct) where T : class
        {
            var resultRowCount = await query.CountAsync(ct);
            if (page == null || pageSize == null)
            {
                var results = await query.ToListAsync(ct);
                return new PagedResult<T>
                {
                    CurrentPage = 1,
                    PageSize = resultRowCount,
                    RowCount = resultRowCount,
                    PageCount = 1,
                    Results = results.ToImmutableList(),
                };
            }

            var pageCount = Math.Ceiling((double)resultRowCount / pageSize.Value);

            var skip = (page.Value - 1) * pageSize.Value;
            var pagedResult = await query.Skip(skip).Take(pageSize.Value).ToListAsync(ct);

            return new PagedResult<T>
            {
                CurrentPage = page.Value,
                PageSize = pageSize.Value,
                RowCount = resultRowCount,
                PageCount = (int)pageCount,
                Results = pagedResult.ToImmutableList()
            };
        }
    }
}
