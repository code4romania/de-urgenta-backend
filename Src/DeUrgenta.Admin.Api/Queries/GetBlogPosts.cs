using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Common.Models.Pagination;
using MediatR;

namespace DeUrgenta.Admin.Api.Queries
{
    public class GetBlogPosts : IRequest<Result<PagedResult<BlogPostModel>>>
    {
        public PaginationQueryModel Pagination { get; }

        public GetBlogPosts(PaginationQueryModel pagination)
        {
            Pagination = pagination;
        }
    }
}