using CSharpFunctionalExtensions;
using MediatR;
using DeUrgenta.Common.Models.Events;
using DeUrgenta.Common.Models.Pagination;
using DeUrgenta.Common.Validation;

namespace DeUrgenta.Admin.Api.Queries
{
    public class GetEvents : IRequest<Result<PagedResult<EventResponseModel>, ValidationResult>>
    {
        public PaginationQueryModel Pagination { get; }

        public GetEvents(PaginationQueryModel pagination)
        {
            Pagination = pagination;
        }
    }
}
