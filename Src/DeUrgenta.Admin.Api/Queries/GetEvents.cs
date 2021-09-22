using CSharpFunctionalExtensions;
using MediatR;
using DeUrgenta.Common.Models;
using DeUrgenta.Common.Models.Events;

namespace DeUrgenta.Admin.Api.Queries
{
    public class GetEvents : IRequest<Result<PagedResult<EventResponseModel>>>
    {
        public PaginationQueryModel Pagination { get; }

        public GetEvents(PaginationQueryModel pagination)
        {
            Pagination = pagination;
        }
    }
}
